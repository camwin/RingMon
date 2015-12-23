'Cameron Paulk
'Class for working with ERPS G.8032 rings on Ciena devices
Imports System
Imports SnmpSharpNet

Public Class CienaSwitchClass

    Private SNMPObj As New mySNMP

    'Vars

    Private mRingID As String
    Private mRingName As String
    Private mRingStatus As String
    Private mRingComment As String
    Private mhostAddr As String

    'Default constructor
    Public Sub New()
        mRingID = ""
        mRingName = ""
        mRingStatus = ""
        mRingComment = ""
        mhostAddr = ""
    End Sub

    'Overloaded constructors
    Public Sub New(pRingID As Integer, pRingName As String, pRingStatus As String, pringComment As String, phostAddr As String)
        mRingID = pRingID
        mRingName = pRingName
        mRingStatus = pRingStatus
        mRingComment = pringComment
        mhostAddr = phostAddr
    End Sub

    'Properties
    Public Property ringID() As Integer
        Get
            Return mRingID
        End Get
        Set(ByVal value As Integer)
            mRingID = value
        End Set
    End Property

    Public Property ringName() As String
        Get
            Return mRingName
        End Get
        Set(ByVal value As String)
            mRingName = value
        End Set
    End Property

    Public Property ringStatus() As String
        Get
            Return mRingStatus
        End Get
        Set(ByVal value As String)
            mRingStatus = value
        End Set
    End Property

    Public Property ringComment() As String
        Get
            Return mRingComment
        End Get
        Set(ByVal value As String)
            mRingComment = value
        End Set
    End Property

    Public Property hostAddr() As String
        Get
            Return mhostAddr
        End Get
        Set(ByVal value As String)
            mhostAddr = value
        End Set
    End Property

    'Structure for holding ring status info.
    Structure ringStatusStructure
        Dim ringNumber As Integer
        Dim ringName As String
        Dim ringStatus As String
        Dim ringComment As String
        Dim hostAddr As String
    End Structure

    'Function to get list of ring statuses from Cisco switch or router
    Public Function GetRAPSRingStatusList(nodeIP As String, community As String) As ringStatusStructure()
        Dim RingIDList As String() = GetRAPRingIDList(nodeIP, community)
        Dim RingNameList As String() = GetRAPRingNameList(nodeIP, community)
        Dim RingStatuses As String() = GetRAPRingStateList(nodeIP, community)

        'Structure to hold ringStatuses
        Dim ringStatusStructure(0) As ringStatusStructure
        Dim count As Integer = 0

        For Each ringNumber In RingIDList

            With ringStatusStructure(count)
                .ringNumber = ringNumber
                .ringName = RingNameList(count)
            End With

            count += 1
            ReDim Preserve ringStatusStructure(ringStatusStructure.Count)
        Next

        'reset counter
        count = 0

        'Ring Status legeng = adminDisabled ( 1 ) , ok ( 2 ) , protecting ( 3 ) , recovering ( 4 ) , init ( 5 ) , none ( 6 )
        For Each i In RingStatuses
            Dim ringStatus As String
            With ringStatusStructure(count)
                If i = 1 Then
                    ringStatus = "Admin Disabled"
                ElseIf i = 2 Then
                    ringStatus = "OK"
                ElseIf i = 3 Then
                    ringStatus = "Protecting"
                ElseIf i = 4 Then
                    ringStatus = "Recovering"
                ElseIf i = 5 Then
                    ringStatus = "init"
                ElseIf i = 6 Then
                    ringStatus = "none"
                End If
                .ringStatus = ringStatus
            End With
            count += 1

        Next

        ReDim Preserve ringStatusStructure(ringStatusStructure.Count - 3)

        Return ringStatusStructure

    End Function

    'Function to get hostname of Cisco Switch
    Public Function GetHostName(nodeIP As String) As String
        Return SNMPObj.SNMPGet(nodeIP, frmMain.txtCommunity.Text, {".1.3.6.1.2.1.1.5.0"})
    End Function

    'Gets a list of ring IDs from a Ciena node
    Public Function GetRAPRingIDList(host As String, community As String) As String()

        Dim ringList(0) As String

        ' Dictionary to store values returned by GetBulk calls
        Dim result As Dictionary(Of Oid, AsnType)
        ' Root Oid for the request (ifTable.ifEntry.ifDescr in this example)
        Dim rootOid As Oid = New Oid(".1.3.6.1.4.1.6141.2.60.47.1.3.1.1.3")
        Dim nextOid As Oid = rootOid
        Dim keepGoing As Boolean = True
        ' Initialize SimpleSnmp class
        Dim snmp As SimpleSnmp = New SimpleSnmp(host, community)
        If Not snmp.Valid Then
            Console.WriteLine("Invalid hostname/community.")
            End
        End If
        ' Set NonRepeaters and MaxRepetitions for the GetBulk request (optional)
        snmp.NonRepeaters = 0
        snmp.MaxRepetitions = 20
        While keepGoing
            ' Make a request
            result = snmp.GetBulk(New String() {nextOid.ToString()})
            ' Check SNMP agent returned valid results
            If result IsNot Nothing Then
                Dim kvp As KeyValuePair(Of Oid, AsnType)
                ' Loop through returned values
                Dim count As Integer = 0
                For Each kvp In result

                    ' Check that returned Oid is part of the original root Oid
                    If rootOid.IsRootOf(kvp.Key) Then

                        'Add ring names to ring list
                        'TO DO - CHECK FOR DUPLICATES
                        ringList(count) = kvp.Value.ToString()
                        ReDim Preserve ringList(ringList.Count)
                        count += 1
                        'MessageBox.Show(kvp.Value.ToString() & " " & count)

                        ' Store last valid Oid to use in additional GetBulk requests (if required)
                        nextOid = kvp.Key
                    Else
                        ' We found a value outside of the root Oid tree. Do not perform additional GetBulk ops
                        keepGoing = False
                    End If
                Next
            Else
                MessageBox.Show("Could not get Ring list from host: " & host)
                keepGoing = False
            End If
        End While
        Return ringList
    End Function


    'Gets a list of ring names
    Public Function GetRAPRingNameList(host As String, community As String) As String()

        Dim ringList(0) As String

        ' Dictionary to store values returned by GetBulk calls
        Dim result As Dictionary(Of Oid, AsnType)
        ' Root Oid for the request (ifTable.ifEntry.ifDescr in this example)
        Dim rootOid As Oid = New Oid(".1.3.6.1.4.1.6141.2.60.47.1.3.1.1.2")
        Dim nextOid As Oid = rootOid
        Dim keepGoing As Boolean = True
        ' Initialize SimpleSnmp class
        Dim snmp As SimpleSnmp = New SimpleSnmp(host, community)
        If Not snmp.Valid Then
            Console.WriteLine("Invalid hostname/community.")
            End
        End If
        ' Set NonRepeaters and MaxRepetitions for the GetBulk request (optional)
        snmp.NonRepeaters = 0
        snmp.MaxRepetitions = 20
        While keepGoing
            ' Make a request
            result = snmp.GetBulk(New String() {nextOid.ToString()})
            ' Check SNMP agent returned valid results
            If result IsNot Nothing Then
                Dim kvp As KeyValuePair(Of Oid, AsnType)
                ' Loop through returned values
                Dim count As Integer = 0
                For Each kvp In result

                    ' Check that returned Oid is part of the original root Oid
                    If rootOid.IsRootOf(kvp.Key) Then

                        'Add ring names to ring list
                        'TO DO - CHECK FOR DUPLICATES
                        ringList(count) = kvp.Value.ToString()
                        ReDim Preserve ringList(ringList.Count)
                        count += 1
                        'MessageBox.Show(kvp.Value.ToString() & " " & count)

                        ' Store last valid Oid to use in additional GetBulk requests (if required)
                        nextOid = kvp.Key
                    Else
                        ' We found a value outside of the root Oid tree. Do not perform additional GetBulk ops
                        keepGoing = False
                    End If
                Next
            Else
                MessageBox.Show("Could not get Ring list from host: " & host)
                keepGoing = False
            End If
        End While
        Return ringList
    End Function


    'Gets a list of ring names
    Public Function GetRAPRingStateList(host As String, community As String) As String()

        Dim ringList(0) As String

        ' Dictionary to store values returned by GetBulk calls
        Dim result As Dictionary(Of Oid, AsnType)
        ' Root Oid for the request (ifTable.ifEntry.ifDescr in this example)
        Dim rootOid As Oid = New Oid(".1.3.6.1.4.1.6141.2.60.47.1.3.1.1.7")
        Dim nextOid As Oid = rootOid
        Dim keepGoing As Boolean = True
        ' Initialize SimpleSnmp class
        Dim snmp As SimpleSnmp = New SimpleSnmp(host, community)
        If Not snmp.Valid Then
            Console.WriteLine("Invalid hostname/community.")
            End
        End If
        ' Set NonRepeaters and MaxRepetitions for the GetBulk request (optional)
        snmp.NonRepeaters = 0
        snmp.MaxRepetitions = 20
        While keepGoing
            ' Make a request
            result = snmp.GetBulk(New String() {nextOid.ToString()})
            ' Check SNMP agent returned valid results
            If result IsNot Nothing Then
                Dim kvp As KeyValuePair(Of Oid, AsnType)
                ' Loop through returned values
                Dim count As Integer = 0
                For Each kvp In result

                    ' Check that returned Oid is part of the original root Oid
                    If rootOid.IsRootOf(kvp.Key) Then

                        'Add ring names to ring list
                        'TO DO - CHECK FOR DUPLICATES
                        ringList(count) = kvp.Value.ToString()
                        ReDim Preserve ringList(ringList.Count)
                        count += 1
                        'MessageBox.Show(kvp.Value.ToString() & " " & count)

                        ' Store last valid Oid to use in additional GetBulk requests (if required)
                        nextOid = kvp.Key
                    Else
                        ' We found a value outside of the root Oid tree. Do not perform additional GetBulk ops
                        keepGoing = False
                    End If
                Next
            Else
                MessageBox.Show("Could not get Ring list from host: " & host)
                keepGoing = False
            End If
        End While
        Return ringList
    End Function


End Class
