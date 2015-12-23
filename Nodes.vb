'Cameron Paulk
' Nodes form, for adding/removing nodes to be queried for rings

Imports System.Xml
Imports System.IO
Imports System.Net
Imports SnmpSharpNet


Public Class Nodes

    'XML File stuff
    Dim settings As New XmlWriterSettings()
    Dim xmlfile As String = frmMain.GetXMLFile()
    Dim xml_doc As New Xml.XmlDocument
    Dim xmlWrt As XmlWriter
    Private SNMPObj As New mySNMP
    Dim community As String = frmMain.txtCommunity.Text

    'Node list of all HostIP elements
    Dim hostIP_child_nodes As XmlNodeList

    'Node list of all Switches
    Dim switches_child_nodes As XmlNodeList

    'Form load event
    Private Sub Nodes_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Get XML settings ready
        settings.Indent = True

        'Check if xml already exists. If so, load it. Otherwise, create it. 

        ' Load the XML file.
        xml_doc.Load(xmlfile)

        ' Sets vars for use targeting specific wrappers
        hostIP_child_nodes = xml_doc.GetElementsByTagName("hostIP")
        switches_child_nodes = xml_doc.GetElementsByTagName("switches")

        RecurseXML_AddHosts(hostIP_child_nodes)
        lstOutput.Sorted = True


    End Sub


    'Add node button click
    Private Sub btnAddNode_Click(sender As Object, e As EventArgs) Handles btnAddNode.Click

        'Check if the thing is even up, and stop the sub if not.
        If My.Computer.Network.Ping(txtNodeIP.Text) Then

        Else
            MsgBox("Ping request timed out. Not adding this one to the list.")
            Exit Sub
        End If

        'Check if the vendor node type is even worth adding
        Dim nodeVendor As String = IdentifyNodeVendor(txtNodeIP.Text)

        'This might no do anything
        If nodeVendor = "Nothing" Then
            MsgBox("This identified as neither a Cisco or Ciena node. Check that it's supported and has SNMP enabled.")
            Exit Sub
        End If

        'Add the switch with the proper vendor attribute
        If nodeVendor = "Cisco" Then
            If IsAddressValid(txtNodeIP.Text) = True Then
                If CheckIfHostIPExists(txtNodeIP.Text, hostIP_child_nodes) = False Then
                    createSwitch(txtNodeIP.Text.ToString(), nodeVendor)
                    lstOutput.Items.Add(txtNodeIP.Text.ToString)
                    txtNodeIP.Clear()

                End If

            Else
                MsgBox("Make sure the IP is in the correct format")
            End If
        ElseIf nodeVendor = "Ciena" Then
            If IsAddressValid(txtNodeIP.Text) = True Then
                If CheckIfHostIPExists(txtNodeIP.Text, hostIP_child_nodes) = False Then
                    createSwitch(txtNodeIP.Text.ToString(), nodeVendor)
                    lstOutput.Items.Add(txtNodeIP.Text.ToString)
                    txtNodeIP.Clear()

                End If

            Else
                MsgBox("Make sure the IP is in the correct format")
            End If

        End If

    End Sub

    'Delete node button click
    Private Sub btnDeleteNode_Click(sender As Object, e As EventArgs) Handles btnDeleteNode.Click

        'Remove hostIP from XML doc
        RemoveEntry(lstOutput.SelectedItem.ToString)
        'Remove node from list box
        Try
            lstOutput.Items.RemoveAt(lstOutput.SelectedIndex)
        Catch ex As Exception

        End Try

    End Sub

    'Sub to create a new switch XML entry
    Private Sub createSwitch(hostIP As String, vendorType As String)

        ' Get a reference to the root node
        Dim RootElement As XmlElement = xml_doc.DocumentElement

        'Create new element for new switch
        Dim newSwitch As XmlElement = xml_doc.CreateElement("switch")

        'Create new element for new switch's vendor type
        Dim newVendorType As XmlElement = xml_doc.CreateElement("vendorType")


        'Set the newHostIP's innertext to the IP address entered
        newVendorType.InnerText = vendorType

        'Create new element for new switch's Host IP
        Dim newHostIP As XmlElement = xml_doc.CreateElement("hostIP")


        'Set the newHostIP's innertext to the IP address entered
        newHostIP.InnerText = hostIP

        'Build the new switch's XML and save
        RootElement.AppendChild(newSwitch)

        newSwitch.AppendChild(newVendorType)

        newSwitch.AppendChild(newHostIP)

        xml_doc.Save(xmlfile)

    End Sub

    'Check if hostIP already in XML
    Public Function CheckIfHostIPExists(ipAddress As String, hostIP_list As XmlNodeList) As Boolean
        For Each host As XmlElement In hostIP_list
            If host.InnerText = txtNodeIP.Text Then
                MsgBox("Host already in the list")
                Return True
            End If
        Next
        Return False
    End Function

    'Steps through entire XML, adding each hostIP 
    Public Sub RecurseXML_AddHosts(nodes As XmlNodeList)
        For Each node As XmlNode In nodes
            If (node.ChildNodes.Count > 0) Then
                RecurseXML_AddHosts(node.ChildNodes)
            Else
                lstOutput.Items.Add(node.InnerText)
            End If
        Next
    End Sub

    'Removes the currently selected node from the XML file
    Public Sub RemoveEntry(match As String)
        ' Get a reference to the root node
        Dim RootElement As XmlElement = xml_doc.DocumentElement

        ' Create a list of the switches
        Dim listOfSwitches As XmlNodeList = xml_doc.GetElementsByTagName("switch")

        ' visit each switch
        For Each switch As XmlNode In listOfSwitches

            ' Within switches tag, get a list of its children, the switches
            Dim ListOfChildren As XmlNodeList = switch.ChildNodes

            ' Visit each child node
            For Each elem As XmlNode In ListOfChildren
                If elem.InnerText = match Then
                    switch.RemoveAll()
                End If

                Exit For
            Next
        Next
        ' Save the file
        xml_doc.Save(xmlfile)
    End Sub

    'Function to check if IP address is a valid one or not 
    Public Function IsAddressValid(ByVal addrString As String) As Boolean
        Dim address As System.Net.IPAddress
        Return System.Net.IPAddress.TryParse(addrString, address)
    End Function

    'Function to ID if node is Cisco or Ciena
    Public Function IdentifyNodeVendor(nodeIP As String) As String
        Dim sysDesc As String = SNMPObj.SNMPGet(nodeIP, community, {".1.3.6.1.2.1.1.1.0"})
        If sysDesc Is Nothing Then
            Return "Nothing"
        End If
        If sysDesc.StartsWith("Cisco") = True Then
            Return "Cisco"
        ElseIf sysDesc.StartsWith("CN") Then
            Return "Ciena"
        ElseIf sysDesc.StartsWith("3930") Then
            Return "Ciena"
        Else
            Console.WriteLine("Found a device that is neither Cisco nor Ciena")
        End If
    End Function

    'Form closing event
    Private Sub nodes_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

    End Sub
End Class
