'Cameron Paulk
' Nodes form, for adding/removing nodes to be queried for rings

Imports System.Xml
Imports System.IO
Imports System.Net
Imports SnmpSharpNet
Imports RingMon.frmMain


Public Class Nodes

    'XML File stuff
    Dim settings As New XmlWriterSettings()
    Dim oShell = CreateObject("Wscript.Shell")
    Dim strUserProfile As String = oShell.ExpandEnvironmentStrings("%USERPROFILE%")
    Dim settings_doc As New Xml.XmlDocument
    Dim settingsFile As String = strUserProfile & "\appdata\local\ringmon_settings.xml"
    Dim xmlfile As String
    Dim xml_doc As New Xml.XmlDocument
    Dim xmlWrt As XmlWriter

    'Node list of all HostIP elements
    Dim hostIP_child_nodes As XmlNodeList

    'Form load event
    Private Sub Nodes_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        settings_doc.Load(settingsFile)
        'Get a reference to the root node
        Dim RootElement As XmlElement = settings_doc.DocumentElement

        'Load the config path
        Dim settingsChildren As XmlNodeList = RootElement.ChildNodes
        For Each element As XmlElement In settingsChildren
            If element.Name = "configPath" Then
                xmlfile = element.InnerText
            End If

        Next

        'load the ringmon XML
        xml_doc.Load(xmlfile)

        'If no ringmon.xml exists in
        ' Load the XML file.
        ' Sets vars for use targeting specific wrappers
        hostIP_child_nodes = xml_doc.GetElementsByTagName("hostIP")
        RecurseXML_AddHosts(hostIP_child_nodes)
        lstOutput.Sorted = True

    End Sub


    'Add node button click
    Private Sub btnAddNode_Click(sender As Object, e As EventArgs) Handles btnAddNode.Click

        If IsAddressValid(txtNodeIP.Text) = True Then
            If CheckHostIP(txtNodeIP.Text, hostIP_child_nodes) = False Then
                createSwitch(txtNodeIP.Text.ToString())
                lstOutput.Items.Add(txtNodeIP.Text.ToString)
                txtNodeIP.Clear()

            End If
            
        Else
            MsgBox("Make sure the IP is in the correct format")
            Exit Sub
        End If

        frmMain.UpdateRings()
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
    Private Sub createSwitch(hostIP As String)

        ' Get a reference to the root node
        Dim RootElement As XmlElement = xml_doc.DocumentElement

        'Create new element for new switch
        Dim newSwitch As XmlElement = xml_doc.CreateElement("switch")

        'Create new element for new switch's Host IP
        Dim newHostIP As XmlElement = xml_doc.CreateElement("hostIP")


        'Set the newHostIP's innertext to the IP address entered
        newHostIP.InnerText = hostIP

        'Build the new switch's XML and save
        RootElement.AppendChild(newSwitch)

        newSwitch.AppendChild(newHostIP)

        xml_doc.Save(xmlfile)

    End Sub

    'Check if hostIP already in XML
    Public Function CheckHostIP(ipAddress As String, hostIP_list As XmlNodeList) As Boolean
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

    'Form closing event
    Private Sub nodes_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

    End Sub

    'Sub for importing a xml file, asking the user where it is
    Private Sub ImportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportToolStripMenuItem.Click
        OpenFileDialog1.Title = "Please Select a File"
        OpenFileDialog1.InitialDirectory = "C:\"
        OpenFileDialog1.ShowDialog()

        xmlfile = OpenFileDialog1.FileName.ToString()

        'Updates the XML path on the main form with what the user just chose
        frmMain.UpdateConfigPath(xmlfile)
        frmMain.updateFilePathLabel()
        ' Load the XML file.
        xml_doc.Load(xmlfile)

        'If no ringmon.xml exists in
        ' Load the XML file.
        ' Sets vars for use targeting specific wrappers
        lstOutput.Items.Clear()
        hostIP_child_nodes = xml_doc.GetElementsByTagName("hostIP")
        RecurseXML_AddHosts(hostIP_child_nodes)
        lstOutput.Sorted = True

        frmMain.UpdateRings()
    End Sub
End Class
