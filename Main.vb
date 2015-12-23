'Cameron Paulk
'Program for monitoring the status of ring topologies like REP and G.8032 ERPS
'8/25/2015
'Requirements:      * = bonus
'   1. Discover Rings - Done
'   2. Keep track of REP ring statuses - Done
'   3. Allow for ring status comments - Done
'   3b. Add support for G.8032 rings - Done
'   4. Create logical map of rings*
'   5. Create logicap map of entire networks*
'   6. Allow for client program to use UNC based file - Done
'   7. Track VLAN trunk consistency*
'   8. Check VLAN database vs trunk allowed*

Imports SnmpSharpNet
Imports System.IO
Imports System
Imports System.Xml

Public Class frmMain
    'Instance vars
    Dim community As String
    Dim ciscoSwitch As New CiscoSwitchClass
    Dim cienaSwitch As New CienaSwitchClass
    Dim sortVar As String = "ringID"

    'XML File stuff
    Dim settings As New XmlWriterSettings()
    Dim xml_doc As New Xml.XmlDocument
    Dim settings_doc As New Xml.XmlDocument
    Dim xmlWrt As XmlWriter
    Dim oShell = CreateObject("Wscript.Shell")
    Dim strUserProfile As String = oShell.ExpandEnvironmentStrings("%USERPROFILE%")
    Dim settingsFile As String = strUserProfile & "\appdata\local\ringmon_settings.xml"
    Dim xmlfile As String = strUserProfile & "\appdata\local\ringmon.xml"

    'Load event
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        'Get XML settings ready
        settings.Indent = True

        cboSort.SelectedIndex = 0

        'Check if xml already exists. If so, load it. Otherwise, create it. 
        If IsFirstRun() = False Then

            ' Load the XML file.
            settings_doc.Load(settingsFile)

            'Get a reference to the root node
            Dim RootElement As XmlElement = settings_doc.DocumentElement
            
            'Load the saved ringmon.xml path and community string
            Dim settingsChildren As XmlNodeList = RootElement.ChildNodes
            For Each element As XmlElement In settingsChildren
                If element.Name = "configPath" Then
                    xmlfile = element.InnerText
                ElseIf element.Name = "community" Then
                    community = element.InnerText
                    txtCommunity.Text = community
                End If

            Next

            UpdateRings()
        Else
            'Ask the user to specify to either import an XML or specify where to save the XML file. Maybe a default to %userprofile%\appdata\local
            MsgBox("Choose an existing XML file, or just click cancel to start a new one locally")

            Dim OFD As New OpenFileDialog()
            With OFD
                .Filter = "All Files |*.*"
                .FileName = ""
                .InitialDirectory = "c:\"
            End With
            If (OFD.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                xmlfile = OFD.FileName
            Else
                MsgBox("Creating your ringmon.xml file at " & xmlfile)
                'Cerate the data file
                xmlWrt = XmlWriter.Create(xmlfile, settings)

                'Create document
                xmlWrt.WriteStartElement("switches")
                xmlWrt.WriteEndElement()
                xmlWrt.WriteEndDocument()
                xmlWrt.Close()

            End If

            community = InputBox("Enter a community string: ", "public", )
            txtCommunity.Text = community.ToString
            UpdateRings()

            'Create the settings file
            xmlWrt = XmlWriter.Create(settingsFile, settings)

            'Create document
            xmlWrt.WriteStartElement("settings")
            xmlWrt.WriteEndElement()
            xmlWrt.WriteEndDocument()
            xmlWrt.Close()

            ' Load the settings file.
            settings_doc.Load(settingsFile)

           ' Get a reference to the root node
            Dim RootElement As XmlElement = settings_doc.DocumentElement

            'Create new element for new switch
            Dim communityString As XmlElement = settings_doc.CreateElement("community")

            'Create new element for new switch's Host IP
            Dim configPath As XmlElement = settings_doc.CreateElement("configPath")

            'Set the newHostIP's innertext to the IP address entered
            configPath.InnerText = xmlfile.ToString

            'Set the community innertext to the IP address entered
            communityString.InnerText = community.ToString

            'Build the new switch's XML and save
            RootElement.AppendChild(communityString)

            RootElement.AppendChild(configPath)

            settings_doc.Save(settingsFile)
        End If
        updateFilePathLabel()
    End Sub


    'Ring maintenance process:
    '   Discover New rings
    '   Discover removed rings
    '   Update ring statuses

    'Sub to query each hostIP for a list of ringIDs and determine whether one has been added or removed
    '1. Get list of all switch elements
    '   2. Loop through each switch element, find the hostIP
    '       3. Get ringStatus list of the hostIP
    '           4. Loop through ringStatus list
    '               5a. If ring isn't already under this switch, add it
    '               5b. otherwise, just update the status of the ring

    Public Sub UpdateRings()

        If txtCommunity.Text = Nothing Then
            MsgBox("Enter a community string to show ring statuses")
            Exit Sub
        Else
            'Load the XML
            xml_doc.Load(xmlfile)

            'Write community to settings XML
            UpdateCommunityString(txtCommunity.Text)

            ' Get reference to document root
            Dim RootElement As XmlElement = xml_doc.DocumentElement

            ' Create a list of nodes whose name is hostIP
            Dim listOfhosts As XmlNodeList = xml_doc.GetElementsByTagName("hostIP")

            ' Create a list of nodes whose name is switch
            Dim listOfSwitches As XmlNodeList = xml_doc.GetElementsByTagName("switch")

            ' Create a list of nodes whose name is switch
            Dim listOfSwitch As XmlNodeList = RootElement.ChildNodes

            ' Create a list of nodes whose name is ringID
            Dim listOfRings As XmlNodeList = xml_doc.GetElementsByTagName("ringID")

            ' Create a list of nodes whose name is ringStatus
            Dim listOfRingsStatuses As XmlNodeList = xml_doc.GetElementsByTagName("ringStatus")

            ' Create a list of nodes whose name is ring
            Dim listofRingsXML As XmlNodeList = xml_doc.GetElementsByTagName("ring")

            Dim hostIP As String = ""
            Dim vendorType As String = ""

            ' visit each switch element
            For Each elem As XmlNode In listOfSwitches

                'Declare a list of ringChildren for
                Dim ringChildren As XmlNodeList

                'visit each element under the switch

                Dim elemChildren As XmlNodeList = elem.ChildNodes

                'Check if switch element is empty, otherwise, keep going
                If elemChildren.Count = Nothing Then
                    'Code should skip empty switch elements
                Else

                    For Each part As XmlElement In elemChildren
                        'Find and set the hostIP
                        If part.Name = "hostIP" Then
                            hostIP = part.InnerText
                        ElseIf part.Name = "vendorType" Then
                            vendorType = part.InnerText
                        ElseIf part.Name = "ring" Then
                            ringChildren = part.ChildNodes
                        End If
                    Next
                    'Check if this is a Cisco switch or a Ciena, so we can query it correctly
                    If vendorType = "Cisco" Then
                        Dim ringList() As CiscoSwitchClass.ringStatusStructure = ciscoSwitch.GetREPRingStatusList(hostIP, community)
                        For Each REPRing In ringList

                            'Check if the ring is already being tracked under this switch node
                            If IsInArray(REPRing.ringNumber, elemChildren) = False Then

                                ' Create an element named ring
                                Dim newRing As XmlElement = xml_doc.CreateElement("ring")
                                ' Get a reference to the parent of the node we have found
                                'Dim switchElement As XmlNode = host.ParentNode
                                ' Add the new element to the node we found
                                elem.AppendChild(newRing)


                                ' Create an element as a child of the new element, for the ringID
                                ' Specify its name as ringID
                                newRing = xml_doc.CreateElement("ringID")
                                ' Create the text of the new element
                                Dim RingNumberText As XmlText = xml_doc.CreateTextNode(REPRing.ringNumber)

                                ' Add the new ringID element to the ring node
                                elem.LastChild.AppendChild(newRing)
                                ' Specify the text of the new node
                                elem.LastChild.LastChild.AppendChild(RingNumberText)


                                ' Create an element as a child of the new element, for the ringStatus
                                ' Specify its name as ringStatus
                                newRing = xml_doc.CreateElement("ringStatus")
                                ' Create the text of the new element
                                Dim RingStatusText As XmlText = xml_doc.CreateTextNode(REPRing.ringComplete)

                                ' Add the new ringStatus element to the ring node
                                elem.LastChild.AppendChild(newRing)
                                ' Specify the text of the new node
                                elem.LastChild.LastChild.AppendChild(RingStatusText)

                                ' Create an element as a child of the new element, for the ringComment
                                ' Specify its name as ringComment
                                newRing = xml_doc.CreateElement("ringComment")
                                ' Create the text of the new element
                                Dim RingCommentText As XmlText = xml_doc.CreateTextNode("")

                                ' Add the new ringStatus element to the ring node
                                elem.LastChild.AppendChild(newRing)
                                ' Specify the text of the new node
                                elem.LastChild.LastChild.AppendChild(RingCommentText)

                            Else
                                'Since the ring already exists on this switch, just update it's ringStatus
                                UpdateRingStatus(REPRing.ringNumber, REPRing.ringComplete, elemChildren)

                            End If
                            xml_doc.Save(xmlfile)
                        Next

                    ElseIf vendorType = "Ciena" Then
                        'Do the same thing here as above, but just for Ciena switches
                        Dim ringList() As CienaSwitchClass.ringStatusStructure = cienaSwitch.GetRAPSRingStatusList(hostIP, community)
                        For Each RAPSring In ringList

                            'Check if the ring is already being tracked under this switch node
                            If IsInArray(RAPSring.ringNumber, elemChildren) = False Then

                                ' Create an element named ring
                                Dim newRing As XmlElement = xml_doc.CreateElement("ring")
                                ' Get a reference to the parent of the node we have found
                                'Dim switchElement As XmlNode = host.ParentNode
                                ' Add the new element to the node we found
                                elem.AppendChild(newRing)


                                ' Create an element as a child of the new element, for the ringID
                                ' Specify its name as ringID
                                newRing = xml_doc.CreateElement("ringID")
                                ' Create the text of the new element
                                Dim RingNumberText As XmlText = xml_doc.CreateTextNode(RAPSring.ringNumber)

                                ' Add the new ringID element to the ring node
                                elem.LastChild.AppendChild(newRing)
                                ' Specify the text of the new node
                                elem.LastChild.LastChild.AppendChild(RingNumberText)

                                ' Create an element as a child of the new element, for the ringName
                                ' Specify its name as ringName
                                newRing = xml_doc.CreateElement("ringName")
                                ' Create the text of the new element
                                Dim RingNameText As XmlText = xml_doc.CreateTextNode(RAPSring.ringName)

                                ' Add the new ringName element to the ring node
                                elem.LastChild.AppendChild(newRing)
                                ' Specify the text of the new node
                                elem.LastChild.LastChild.AppendChild(RingNameText)


                                ' Create an element as a child of the new element, for the ringStatus
                                ' Specify its name as ringStatus
                                newRing = xml_doc.CreateElement("ringStatus")
                                ' Create the text of the new element
                                Dim RingStatusText As XmlText = xml_doc.CreateTextNode(RAPSring.ringStatus)

                                ' Add the new ringStatus element to the ring node
                                elem.LastChild.AppendChild(newRing)
                                ' Specify the text of the new node
                                elem.LastChild.LastChild.AppendChild(RingStatusText)

                                ' Create an element as a child of the new element, for the ringComment
                                ' Specify its name as ringComment
                                newRing = xml_doc.CreateElement("ringComment")
                                ' Create the text of the new element
                                Dim RingCommentText As XmlText = xml_doc.CreateTextNode("")

                                ' Add the new ringStatus element to the ring node
                                elem.LastChild.AppendChild(newRing)
                                ' Specify the text of the new node
                                elem.LastChild.LastChild.AppendChild(RingCommentText)

                            Else
                                'Since the ring already exists on this switch, just update it's ringStatus
                                UpdateRAPSRingStatus(RAPSring.ringNumber, RAPSring.ringStatus, elemChildren)

                            End If
                            xml_doc.Save(xmlfile)
                        Next

                    End If

                End If

            Next

            updateFilePathLabel()

            XML_List_to_dgv(listOfSwitch)

        End If
    End Sub

    'Check if string is in a given XMLNodeList
    Public Sub UpdateRingStatus(ringNumber As String, ringStatus As String, ringList As XmlNodeList)
        For Each thing As XmlNode In ringList
            If thing.Name = "ring" Then
                Dim ringChildren As XmlNodeList = thing.ChildNodes
                For Each item As XmlElement In ringChildren
                    If item.InnerText = ringNumber Then
                        If item.NextSibling.InnerText <> ringStatus Then
                            item.NextSibling.InnerText = ringStatus
                        End If

                    End If
                Next
            End If
        Next

    End Sub

    'Check if string is in a given XMLNodeList
    Public Sub UpdateRAPSRingStatus(ringNumber As String, ringStatus As String, ringList As XmlNodeList)
        For Each thing As XmlNode In ringList
            If thing.Name = "ring" Then
                Dim ringChildren As XmlNodeList = thing.ChildNodes
                For Each item As XmlElement In ringChildren
                    If item.InnerText = ringNumber Then
                        If item.NextSibling.NextSibling.InnerText <> ringStatus Then
                            item.NextSibling.NextSibling.InnerText = ringStatus
                        End If

                    End If
                Next
            End If
        Next

    End Sub

    Public Function IsFirstRun() As Boolean
        If File.Exists(settingsFile) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Sub XML_List_to_dgv(list As XmlNodeList)
        'To output entire list of rings to DGV, with columns for Ring ID, Ring Status, and Comment:
        '   Get list of all Rings
        '   Loop through list of rings
        '       For each ring in list, find the status by getting a reference to the parent node
        '           then getting the status beneath that parent by looping through children until you find the ringStatus element
        Dim vendorType As String = ""
        Dim hostAddr As String = ""
        Dim hostName As String = ""
        dgvOutput.DataSource = Nothing

        Dim allCiscoRingsList As New List(Of CiscoSwitchClass)
        Dim allCienaRingsList As New List(Of CienaSwitchClass)

        'Loop through each switch
        For Each switch As XmlNode In list
            'Loop through ecah switches children
            Dim listofSwitchChildren As XmlNodeList = switch.ChildNodes
            For Each switchChild As XmlNode In listofSwitchChildren
                'Set the vendor type
                If switchChild.Name = "vendorType" Then
                    vendorType = switchChild.InnerText
                End If

                If switchChild.Name = "hostIP" Then
                    hostAddr = switchChild.InnerText

                    'Update the hostName.
                    hostName = cienaSwitch.GetHostName(hostAddr)
                End If

                'Then decide on which rings we need to show. If REP radio is selected, output for REP, etc...

                If rdoREP.Checked = True Then

                    If switchChild.Name = "ring" Then
                        If vendorType = "Cisco" Then
                            Dim ringChildren As XmlNodeList = switchChild.ChildNodes
                            Dim ringID As Integer
                            Dim ringStatus As String = ""
                            Dim ringComment As String = ""
                            For Each ringElement As XmlNode In ringChildren

                                If ringElement.Name = "ringID" Then
                                    ringID = CInt(ringElement.InnerText)
                                ElseIf ringElement.Name = "ringStatus" Then
                                    ringStatus = ringElement.InnerText.ToString
                                ElseIf ringElement.Name = "ringComment" Then
                                    ringComment = ringElement.InnerText.ToString
                                End If

                            Next
                            allCiscoRingsList.Add(New CiscoSwitchClass(ringID, StatusToString(ringStatus), ringComment, hostName))
                        End If
                        
                    End If

                ElseIf rdoRAPS.Checked = True Then

                    If switchChild.Name = "ring" Then
                        If vendorType = "Ciena" Then
                            Dim ringChildren As XmlNodeList = switchChild.ChildNodes
                            Dim ringID As Integer
                            Dim ringName As String = ""
                            Dim ringStatus As String = ""
                            Dim ringComment As String = ""
                            For Each ringElement As XmlNode In ringChildren

                                If ringElement.Name = "ringID" Then
                                    ringID = CInt(ringElement.InnerText)
                                ElseIf ringElement.Name = "ringName" Then
                                    ringName = ringElement.InnerText.ToString
                                ElseIf ringElement.Name = "ringStatus" Then
                                    ringStatus = ringElement.InnerText.ToString
                                ElseIf ringElement.Name = "ringComment" Then
                                    ringComment = ringElement.InnerText.ToString
                                End If

                            Next
                            allCienaRingsList.Add(New CienaSwitchClass(ringID, ringName, ringStatus, ringComment, hostName))
                        End If

                    End If

                End If

            Next
            
        Next

        QueryToDGV(sortVar, allCiscoRingsList, allCienaRingsList)


        'Finally, color the rows that say "BROKEN" red, so they stand out
        ColorRows()

        'Fianlly, finally, resize the col
        For i = 0 To dgvOutput.ColumnCount - 1
            dgvOutput.Columns(i).HeaderCell.Style.Alignment() = DataGridViewContentAlignment.BottomCenter
        Next

    End Sub

    'Sub to output query to DGV based on sortVar
    Public Sub QueryToDGV(sortVar As String, ciscoRingsList As List(Of CiscoSwitchClass), cienaRingsList As List(Of CienaSwitchClass))

        If rdoREP.Checked = True Then

            If sortVar = "ringID" Then
                Dim ringQuery = From all In ciscoRingsList
                            Let ringID = all.ringID
                            Let ringStatus = all.ringStatus
                            Let ringComment = all.ringComment
                            Let hostAddr = all.hostAddr
                            Select hostAddr, ringID, ringStatus, ringComment
                            Order By ringID

                dgvOutput.DataSource = ringQuery.ToList
                dgvOutput.Columns("hostAddr").HeaderText = "Host Name/IP"
                dgvOutput.Columns("ringID").HeaderText = "Ring Number"
                dgvOutput.Columns("ringStatus").HeaderText = "Ring Status"
                dgvOutput.Columns("ringComment").HeaderText = "Comment"
                dgvOutput.RowHeadersVisible = False

            ElseIf sortVar = "ringStatus" Then
                Dim ringQuery = From all In ciscoRingsList
                            Let ringID = all.ringID
                            Let ringStatus = all.ringStatus
                            Let ringComment = all.ringComment
                            Let hostAddr = all.hostAddr
                            Select hostAddr, ringID, ringStatus, ringComment
                            Order By ringStatus

                dgvOutput.DataSource = ringQuery.ToList
                dgvOutput.Columns("hostAddr").HeaderText = "Host Name/IP"
                dgvOutput.Columns("ringID").HeaderText = "Ring Number"
                dgvOutput.Columns("ringStatus").HeaderText = "Ring Status"
                dgvOutput.Columns("ringComment").HeaderText = "Comment"
                dgvOutput.RowHeadersVisible = False

            Else
                Dim ringQuery = From all In ciscoRingsList
                            Let ringID = all.ringID
                            Let ringStatus = all.ringStatus
                            Let ringComment = all.ringComment
                            Let hostAddr = all.hostAddr
                            Select hostAddr, ringID, ringStatus, ringComment
                            Order By hostAddr

                dgvOutput.DataSource = ringQuery.ToList
                dgvOutput.Columns("hostAddr").HeaderText = "Host Name/IP"
                dgvOutput.Columns("ringID").HeaderText = "Ring Number"
                dgvOutput.Columns("ringStatus").HeaderText = "Ring Status"
                dgvOutput.Columns("ringComment").HeaderText = "Comment"
                dgvOutput.RowHeadersVisible = False

            End If

        ElseIf rdoRAPS.Checked = True Then

            If sortVar = "ringID" Then
                Dim ringQuery = From all In cienaRingsList
                            Let ringID = all.ringID
                            Let ringName = all.ringName
                            Let ringStatus = all.ringStatus
                            Let ringComment = all.ringComment
                            Let hostAddr = all.hostAddr
                            Select hostAddr, ringID, ringName, ringStatus, ringComment
                            Order By ringID

                dgvOutput.DataSource = ringQuery.ToList
                dgvOutput.Columns("hostAddr").HeaderText = "Host Name/IP"
                dgvOutput.Columns("ringName").HeaderText = "Ring Name"
                dgvOutput.Columns("ringID").HeaderText = "Ring Number"
                dgvOutput.Columns("ringStatus").HeaderText = "Ring Status"
                dgvOutput.Columns("ringComment").HeaderText = "Comment"
                dgvOutput.RowHeadersVisible = False

            ElseIf sortVar = "ringStatus" Then
                Dim ringQuery = From all In cienaRingsList
                            Let ringID = all.ringID
                            Let ringName = all.ringName
                            Let ringStatus = all.ringStatus
                            Let ringComment = all.ringComment
                            Let hostAddr = all.hostAddr
                            Select hostAddr, ringID, ringName, ringStatus, ringComment
                            Order By ringStatus

                dgvOutput.DataSource = ringQuery.ToList
                dgvOutput.Columns("hostAddr").HeaderText = "Host Name/IP"
                dgvOutput.Columns("ringName").HeaderText = "Ring Name"
                dgvOutput.Columns("ringID").HeaderText = "Ring Number"
                dgvOutput.Columns("ringStatus").HeaderText = "Ring Status"
                dgvOutput.Columns("ringComment").HeaderText = "Comment"
                dgvOutput.RowHeadersVisible = False

            Else
                Dim ringQuery = From all In cienaRingsList
                            Let ringID = all.ringID
                            Let ringName = all.ringName
                            Let ringStatus = all.ringStatus
                            Let ringComment = all.ringComment
                            Let hostAddr = all.hostAddr
                            Select hostAddr, ringID, ringName, ringStatus, ringComment
                            Order By hostAddr

                dgvOutput.DataSource = ringQuery.ToList
                dgvOutput.Columns("hostAddr").HeaderText = "Host Name/IP"
                dgvOutput.Columns("ringName").HeaderText = "Ring Name"
                dgvOutput.Columns("ringID").HeaderText = "Ring Number"
                dgvOutput.Columns("ringStatus").HeaderText = "Ring Status"
                dgvOutput.Columns("ringComment").HeaderText = "Comment"
                dgvOutput.RowHeadersVisible = False

            End If

        End If

    End Sub

    'Edit comment button click
    Private Sub btnEditComment_Click(sender As Object, e As EventArgs) Handles btnEditComment.Click
        Dim newComment As String = InputBox("Enter a comment for the ring. Leave empty to clear the comment.", "Edit comment")

        ' Create a list of nodes whose name is ringID
        Dim listOfRingIDs As XmlNodeList = xml_doc.GetElementsByTagName("ringID")

        ' Create a list of nodes whose name is ringID
        Dim listOfRingNames As XmlNodeList = xml_doc.GetElementsByTagName("ringName")

        If rdoREP.Checked = True Then
            '' visit each hostIP
            For Each ring As XmlNode In listOfRingIDs

                If ring.InnerText = dgvOutput.Item(1, dgvOutput.CurrentRow.Index).Value.ToString Then

                    'Get a reference to the parent
                    Dim ringParent As XmlNode = ring.ParentNode
                    For Each child As XmlNode In ringParent.ChildNodes
                        If child.Name = "ringComment" Then
                            child.InnerText = newComment.ToString
                            xml_doc.Save(xmlfile)
                            Exit For
                        End If
                    Next
                End If

            Next
        ElseIf rdoRAPS.Checked = True Then
            '' visit each hostIP
            For Each ringName As XmlNode In listOfRingNames

                If ringName.InnerText = dgvOutput.Item(2, dgvOutput.CurrentRow.Index).Value.ToString Then

                    'Get a reference to the parent
                    Dim ringParent As XmlNode = ringName.ParentNode
                    For Each child As XmlNode In ringParent.ChildNodes
                        If child.Name = "ringComment" Then
                            child.InnerText = newComment.ToString
                            xml_doc.Save(xmlfile)
                            Exit For
                        End If
                    Next
                End If

            Next

        End If


       
        UpdateRings()
    End Sub

    'Iterates through dgvOutput, coloring rows red is they are protecting, yellow if they are recovering.
    Public Sub ColorRows()
        'counter to use for updating a label with # of broken rings
        Dim counter As Integer = 0
        For dgvrow As Integer = 0 To dgvOutput.Rows.Count - 1
            If dgvOutput.Rows(dgvrow).Cells(0).Value Is Nothing Then
                Exit For
            End If

            If rdoREP.Checked = True Then
                If dgvOutput.Rows(dgvrow).Cells(2).Value.ToString.Contains("BROKEN") = True Then
                    dgvOutput.Rows(dgvrow).DefaultCellStyle.BackColor = Color.Red
                    counter += 1
                End If
            End If
            
            If rdoRAPS.Checked = True Then
                If dgvOutput.Rows(dgvrow).Cells(3).Value.ToString.Contains("Protecting") = True Then
                    dgvOutput.Rows(dgvrow).DefaultCellStyle.BackColor = Color.Red
                    counter += 1
                End If
            End If
        Next
        lblBrokenRings.Text = "Broken Rings: " & counter
    End Sub

    'Check if string is in a given XMLNodeList
    Public Function IsInArray(checkThis As String, inthis As XmlNodeList) As Boolean
        For Each thing As XmlNode In inthis
            If thing.Name = "ring" Then
                Dim thingChildren As XmlNodeList = thing.ChildNodes
                For Each thingChild As XmlElement In thingChildren
                    If thingChild.Name = "ringID" Then
                        If thingChild.InnerText = checkThis Then
                            Return True
                        End If
                    End If
                Next
            End If
        Next
        Return False
    End Function


    Public Function StatusToString(status As String) As String
        If status = "True" Then
            Return "OK"
        ElseIf status = "False" Then
            Return "BROKEN"
        End If
        Return False
    End Function

    Public Sub ClearXML()
        ' Get a reference to the root node
        Dim RootElement As XmlElement = xml_doc.DocumentElement

        ' Create a list of nodes whose name is switches
        Dim listofswitches As XmlNodeList = xml_doc.GetElementsByTagName("switch")

        '' visit each switch
        For Each Switch As XmlNode In listofswitches

            'visit each switch
            Dim listofChildren As XmlNodeList = Switch.ChildNodes

            'visit each child
            For Each child As XmlNode In listofChildren
                If child.Name = "ring" Then
                    Switch.RemoveChild(child)

                    xml_doc.Save(xmlfile)
                End If
            Next

        Next


    End Sub

    'Steps through entire XML, adding each hostIP 
    Public Sub RecurseXML_List(nodes As XmlNodeList)
        MsgBox(nodes.Count.ToString)
        For Each node As XmlNode In nodes
            For Each node1 As XmlNode In nodes
                If (node1.ChildNodes.Count > 0) Then
                    MsgBox(node.Name.ToString)
                    node1.ParentNode.RemoveChild(node1)
                End If

            Next
        Next
    End Sub


    Public Sub updateFilePathLabel()
        lblConfigPath.Text = "Config: " & xmlfile
        lblConfigPath.Visible = True
    End Sub

    Public Sub UpdateCommunityString(newCommunity As String)
        'Write community to settings XML
        Dim communityList As XmlNodeList = settings_doc.GetElementsByTagName("community")
        For Each comm As XmlElement In communityList
            If comm.Name = "community" Then
                Try
                    comm.InnerText = newCommunity.ToString
                    community = newCommunity.ToString
                    settings_doc.Save(settingsFile)
                Catch ex As Exception
                    MsgBox(ErrorToString)
                End Try
            End If
        Next

    End Sub

    Public Sub UpdateConfigPath(newPath As String)
        'Write community to settings XML
        Dim communityList As XmlNodeList = settings_doc.GetElementsByTagName("configPath")
        For Each comm As XmlElement In communityList
            If comm.Name = "configPath" Then
                Try
                    comm.InnerText = newPath.ToString
                    settings_doc.Save(settingsFile)
                Catch ex As Exception
                    MsgBox(ErrorToString)
                End Try
            End If
        Next
        xmlfile = newPath
    End Sub

    Public Sub UpdateSortVar()
        sortVar = "hostAddr"

        If cboSort.SelectedIndex = 0 Then
            sortVar = "ringID"

        ElseIf cboSort.SelectedIndex = 1 Then
            sortVar = "hostAddr"
        Else
            sortVar = "ringStatus"

        End If
    End Sub


    'Open nodes form
    Private Sub NodesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NodesToolStripMenuItem.Click
        Nodes.Show()
    End Sub

    'Quit menu option
    Private Sub QuitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        UpdateRings()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        frmAbout.Show()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSort.SelectedIndexChanged
        UpdateSortVar()
    End Sub

    Private Sub ConfigToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigToolStripMenuItem.Click
        'Ask the user to specify to either import an XML or specify where to save the XML file. Maybe a default to %userprofile%\appdata\local
        MsgBox("Choose an existing XML file.")

        Dim OFD As New OpenFileDialog()
        With OFD
            .Filter = "All Files |*.*"
            .FileName = ""
            .InitialDirectory = "c:\"
        End With
        If (OFD.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            xmlfile = OFD.FileName

        Else
            MsgBox("Config path still: " & xmlfile)
            Exit Sub
        End If

        'Save the new config path to the settings file
        ' Create a list of nodes whose name is hostIP
        Dim configPathList As XmlNodeList = settings_doc.GetElementsByTagName("configPath")
        For Each child As XmlElement In configPathList
            child.InnerText = xmlfile
        Next

        settings_doc.Save(settingsFile)

        'Finally, update the ring list for the new config file
        UpdateRings()
    End Sub

    'Function to get the xmlfile
    Public Function GetXMLFile() As String
        Return xmlfile
    End Function
End Class
