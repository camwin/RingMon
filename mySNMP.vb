Imports SnmpSharpNet
Imports System

Public Class mySNMP

    Public Function SNMPGet(nodeIP As String, community As String, oid As String()) As String

        Dim result As Dictionary(Of Oid, AsnType)
        Dim snmp As SimpleSnmp = New SimpleSnmp(nodeIP, community)
        If Not snmp.Valid Then
            MessageBox.Show("Invalid hostname/community")
            Return False
            Exit Function
        End If
        result = snmp.Get(SnmpVersion.Ver1, oid)
        If result IsNot Nothing Then
            Dim kvp As KeyValuePair(Of Oid, AsnType)
            For Each kvp In result
                'MessageBox.Show(kvp.Value.ToString())
                Return kvp.Value.ToString()
            Next
        Else
            Return False
            'MessageBox.Show("No results received.")
        End If
        Return False
    End Function

End Class