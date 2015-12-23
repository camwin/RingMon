Imports SnmpSharpNet
Imports System

Public Class CiscoSwitchClass
    Private SNMPObj As New mySNMP

    'Vars

    Private mRingID As String
    Private mRingStatus As String
    Private mRingComment As String
    Private mhostAddr As String

    'Default constructor
    Public Sub New()
        mRingID = ""
        mRingStatus = ""
        mRingComment = ""
        mhostAddr = ""
    End Sub

    'Overloaded constructors
    Public Sub New(pRingID As Integer, pRingStatus As String, pringComment As String, phostAddr As String)
        mRingID = pRingID
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
        Dim ringComplete As Boolean
        Dim ringComment As String
        Dim hostAddr As String
    End Structure


    'Function to get list of rings from Cisco switch or router
    Public Function GetREPRingList(nodeIP As String, community As String) As Integer()
        Dim RingList As String() = GetSNMPBulkKey(nodeIP, community, ".1.3.6.1.4.1.9.9.601.1.2.1.1.2")

        'Convert to integers
        Dim listofInts(0) As Integer
        For Each ring In RingList

            listofInts(listofInts.Count - 1) = CInt(ring.ToString)
            ReDim Preserve listofInts(listofInts.Count)
        Next

        'Remove extra array spot
        ReDim Preserve listofInts(listofInts.Count - 2)

        Array.Sort(listofInts)

        Return listofInts
    End Function


    'Function to get list of ring statuses from Cisco switch or router
    Public Function GetREPRingStatusList(nodeIP As String, community As String) As ringStatusStructure()
        Dim RingList As String() = GetSNMPBulk(nodeIP, community, ".1.3.6.1.4.1.9.9.601.1.3.1.1.4")
        Dim RingStatuses As String() = GetSNMPBulkKey(nodeIP, community, ".1.3.6.1.4.1.9.9.601.1.3.1.1.4")

        'Structure to hold ringStatuses
        Dim ringStatusStructure(0) As ringStatusStructure
        Dim count As Integer = 0

        For Each ringNumber In RingList

            With ringStatusStructure(count)
                .ringNumber = ringNumber
            End With
            count += 1
            ReDim Preserve ringStatusStructure(ringStatusStructure.Count)
        Next

        'reset counter
        count = 0

        For Each i In RingStatuses
            Dim ringStatusBool As Boolean
            With ringStatusStructure(count)
                If i = 1 Then
                    ringStatusBool = True
                Else
                    ringStatusBool = False
                End If
                .ringComplete = ringStatusBool
            End With
            count += 1
        Next

        ReDim Preserve ringStatusStructure(ringStatusStructure.Count - 2)

        Return ringStatusStructure

    End Function

    'Function to get hostname of Cisco Switch
    Public Function GetCiscoHostName(nodeIP As String) As String
        Return SNMPObj.SNMPGet(nodeIP, frmMain.txtCommunity.Text, {".1.3.6.1.4.1.9.2.1.3.0"})
    End Function

    Public Function GetTrunkVLANs(nodeIP As String, community As String, range As Integer) As String
        Dim REPPorts() = GetSNMPBulk(nodeIP, community, ".1.3.6.1.4.1.9.9.601.1.2.1.1.2")
        Dim OnekVLANsOid As String = ".1.3.6.1.4.1.9.9.46.1.6.1.1.4." & REPPorts(0)
        Dim TwokVLANsOid As String = ".1.3.6.1.4.1.9.9.46.1.6.1.1.17." & REPPorts(0)
        Dim ThreekVLANsOid As String = ".1.3.6.1.4.1.9.9.46.1.6.1.1.18." & REPPorts(0)
        Dim FourkVLANsOid As String = ".1.3.6.1.4.1.9.9.46.1.6.1.1.19." & REPPorts(0)


        If range = 1 Then
            Return SNMPObj.SNMPGet(nodeIP, community, {OnekVLANsOid})
        End If
        If range = 2 Then
            Return SNMPObj.SNMPGet(nodeIP, community, {TwokVLANsOid})
        End If
        If range = 3 Then
            Return SNMPObj.SNMPGet(nodeIP, community, {ThreekVLANsOid})
        End If
        If range = 4 Then
            Return SNMPObj.SNMPGet(nodeIP, community, {FourkVLANsOid})
        End If
        Return False
    End Function

    Public Function GetVLANsAllowed(node As String, community As String)
        'To get a list of the VLANs written to a trunk, read out the 256 bit string and decode it. 
        'Use the SNMP notes for the OIDs to decipher
        Dim trunkVLANs1023 As String = GetTrunkVLANs(node, community, 1)
        Dim trunkVLANs2047 As String = GetTrunkVLANs(node, community, 2)
        Dim trunkVLANs3071 As String = GetTrunkVLANs(node, community, 3)
        Dim trunkVLANs4095 As String = GetTrunkVLANs(node, community, 4)



        Dim vlansAllowed(0) As Integer
        Dim vlanMultiplier As Integer

        'Check VLANs 0-1023
        vlanMultiplier = 0

        For Each i As Char In trunkVLANs1023

            Dim vlanAdder As Integer = 0

            If i <> " " Then
                If i <> "0" Then

                    If i = "1" Then
                        vlanAdder = 4
                    ElseIf i = "2" Then
                        vlanAdder = 3
                    ElseIf i = "4" Then
                        vlanAdder = 2
                    Else
                        vlanAdder = 1
                    End If

                    'convert found bit to number
                    'MessageBox.Show(((vlanMultiplier * 4) + vlanAdder) - 1)
                    vlansAllowed(vlansAllowed.Count - 1) = ((vlanMultiplier * 4) + vlanAdder) - 1
                    ReDim Preserve vlansAllowed(vlansAllowed.Count)

                End If
                vlanMultiplier += 1

            End If

        Next

        'Check VLANs 1024 through 2047
        vlanMultiplier = 256

        For Each i As Char In trunkVLANs2047

            Dim vlanAdder As Integer = 1023

            If i <> " " Then
                If i <> "0" Then

                    If i = "1" Then
                        vlanAdder = 4
                    ElseIf i = "2" Then
                        vlanAdder = 3
                    ElseIf i = "4" Then
                        vlanAdder = 2
                    Else
                        vlanAdder = 1

                    End If

                    'convert found bit to number
                    'MessageBox.Show(((vlanMultiplier * 4) + vlanAdder) - 1)
                    vlansAllowed(vlansAllowed.Count - 1) = ((vlanMultiplier * 4) + vlanAdder) - 1
                    ReDim Preserve vlansAllowed(vlansAllowed.Count)

                End If
                vlanMultiplier += 1

            End If

        Next

        'Check VLANs 2048 through 3071
        vlanMultiplier = 512

        For Each i As Char In trunkVLANs3071

            Dim vlanAdder As Integer = 2047

            If i <> " " Then
                If i <> "0" Then

                    If i = "1" Then
                        vlanAdder = 4
                    ElseIf i = "2" Then
                        vlanAdder = 3
                    ElseIf i = "4" Then
                        vlanAdder = 2
                    Else
                        vlanAdder = 1

                    End If

                    'convert found bit to number
                    'MessageBox.Show(((vlanMultiplier * 4) + vlanAdder) - 1)
                    vlansAllowed(vlansAllowed.Count - 1) = ((vlanMultiplier * 4) + vlanAdder) - 1
                    ReDim Preserve vlansAllowed(vlansAllowed.Count)

                End If
                vlanMultiplier += 1

            End If

        Next

        'Check 3072 through 4095
        vlanMultiplier = 768

        For Each i As Char In trunkVLANs4095

            Dim vlanAdder As Integer = 3071

            If i <> " " Then
                If i <> "0" Then

                    If i = "1" Then
                        vlanAdder = 4
                    ElseIf i = "2" Then
                        vlanAdder = 3
                    ElseIf i = "4" Then
                        vlanAdder = 2
                    Else
                        vlanAdder = 1

                    End If

                    'convert found bit to number
                    'MessageBox.Show(((vlanMultiplier * 4) + vlanAdder) - 1)
                    vlansAllowed(vlansAllowed.Count - 1) = ((vlanMultiplier * 4) + vlanAdder) - 1
                    ReDim Preserve vlansAllowed(vlansAllowed.Count)

                End If
                vlanMultiplier += 1

            End If

        Next
        ReDim Preserve vlansAllowed(UBound(vlansAllowed) - 1)
        Return vlansAllowed


    End Function

    Public Function GetSNMPBulk(host As String, community As String, oid As String) As String()
        Dim returnedStrings(0) As String

        ' Dictionary to store values returned by GetBulk calls
        Dim result As Dictionary(Of Oid, AsnType)
        ' Root Oid for the request (ifTable.ifEntry.ifDescr in this example)
        Dim rootOid As Oid = New Oid(oid)
        Dim nextOid As Oid = rootOid
        Dim keepGoing As Boolean = True
        ' Initialize SimpleSnmp class
        Dim snmp As SimpleSnmp = New SimpleSnmp(host, community)
        If Not snmp.Valid Then
            Console.WriteLine("Invalid hostname/community.")
            End
        End If
        ' Set NonRepeaters and MaxRepetitions for the GetBulk request (optional)
        'snmp.NonRepeaters = 9999
        'snmp.MaxRepetitions = 9999
        Dim count As Integer = 0
        While keepGoing
            ' Make a request
            result = snmp.GetBulk(New String() {nextOid.ToString()})
            ' Check SNMP agent returned valid results
            If result IsNot Nothing Then
                Dim kvp As KeyValuePair(Of Oid, AsnType)
                ' Loop through returned values
                For Each kvp In result

                    ' Check that returned Oid is part of the original root Oid
                    If rootOid.IsRootOf(kvp.Key) Then

                        Dim oidSplit() As String = Split(kvp.Key.ToString(), ".")
                        returnedStrings(count) = oidSplit(oidSplit.Count - 1)
                        ReDim Preserve returnedStrings(returnedStrings.Count)
                        count += 1
                        'MessageBox.Show(kvp.Key.ToString() & " " & count)

                        ' Store last valid Oid to use in additional GetBulk requests (if required)
                        nextOid = kvp.Key
                    Else
                        ' We found a value outside of the root Oid tree. Do not perform additional GetBulk ops
                        keepGoing = False
                    End If
                Next
            Else
                MessageBox.Show("Could not get REP Ports list from host: " & host & ". Probably bad community string.")
                keepGoing = False
            End If
        End While
        ReDim Preserve returnedStrings(UBound(returnedStrings) - 1)
        Return returnedStrings
    End Function

    Public Function GetSNMPBulkKey(host As String, community As String, oid As String) As String()
        Dim returnedStrings(0) As String

        ' Dictionary to store values returned by GetBulk calls
        Dim result As Dictionary(Of Oid, AsnType)
        ' Root Oid for the request (ifTable.ifEntry.ifDescr in this example)
        Dim rootOid As Oid = New Oid(oid)
        Dim nextOid As Oid = rootOid
        Dim keepGoing As Boolean = True
        ' Initialize SimpleSnmp class
        Dim snmp As SimpleSnmp = New SimpleSnmp(host, community)
        If Not snmp.Valid Then
            Console.WriteLine("Invalid hostname/community.")
            End
        End If
        ' Set NonRepeaters and MaxRepetitions for the GetBulk request (optional)
        'snmp.NonRepeaters = 9999
        'snmp.MaxRepetitions = 9999
        Dim count As Integer = 0
        While keepGoing
            ' Make a request
            result = snmp.GetBulk(New String() {nextOid.ToString()})
            ' Check SNMP agent returned valid results
            If result IsNot Nothing Then
                Dim kvp As KeyValuePair(Of Oid, AsnType)
                ' Loop through returned values
                For Each kvp In result

                    ' Check that returned Oid is part of the original root Oid
                    If rootOid.IsRootOf(kvp.Key) Then

                        Dim oidSplit() As String = Split(kvp.Key.ToString(), ".")
                        returnedStrings(count) = kvp.Value.ToString()
                        ReDim Preserve returnedStrings(returnedStrings.Count)
                        count += 1
                        'MessageBox.Show(kvp.Key.ToString() & " " & count)

                        ' Store last valid Oid to use in additional GetBulk requests (if required)
                        nextOid = kvp.Key
                    Else
                        ' We found a value outside of the root Oid tree. Do not perform additional GetBulk ops
                        keepGoing = False
                    End If
                Next
            Else
                MessageBox.Show("Could not get REP Ports list from host: " & host & ". Probably bad community string.")
                keepGoing = False
            End If
        End While
        ReDim Preserve returnedStrings(UBound(returnedStrings) - 1)
        Return returnedStrings
    End Function

End Class
