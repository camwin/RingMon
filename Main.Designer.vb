<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NodesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QuitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtCommunity = New System.Windows.Forms.TextBox()
        Me.dgvOutput = New System.Windows.Forms.DataGridView()
        Me.btnEditComment = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.lblBrokenRings = New System.Windows.Forms.Label()
        Me.lblConfigPath = New System.Windows.Forms.Label()
        Me.lblSortBy = New System.Windows.Forms.Label()
        Me.cboSort = New System.Windows.Forms.ComboBox()
        Me.rdoREP = New System.Windows.Forms.RadioButton()
        Me.rdoRAPS = New System.Windows.Forms.RadioButton()
        Me.lblLastUpdated = New System.Windows.Forms.Label()
        Me.lblTotalRings = New System.Windows.Forms.Label()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.dgvOutput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Nirmala UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(177, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(222, 45)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Ring Monitor"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(579, 24)
        Me.MenuStrip1.TabIndex = 5
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NodesToolStripMenuItem, Me.ConfigToolStripMenuItem, Me.QuitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NodesToolStripMenuItem
        '
        Me.NodesToolStripMenuItem.Name = "NodesToolStripMenuItem"
        Me.NodesToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.NodesToolStripMenuItem.Text = "Nodes"
        '
        'ConfigToolStripMenuItem
        '
        Me.ConfigToolStripMenuItem.Name = "ConfigToolStripMenuItem"
        Me.ConfigToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.ConfigToolStripMenuItem.Text = "Config..."
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem"
        Me.QuitToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.QuitToolStripMenuItem.Text = "Quit"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.AboutToolStripMenuItem.Text = "About..."
        '
        'btnUpdate
        '
        Me.btnUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdate.Font = New System.Drawing.Font("Nirmala UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdate.Location = New System.Drawing.Point(492, 359)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdate.TabIndex = 1
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Nirmala UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.Label8.Location = New System.Drawing.Point(15, 364)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(155, 21)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "SNMP Community:"
        '
        'txtCommunity
        '
        Me.txtCommunity.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtCommunity.Location = New System.Drawing.Point(173, 365)
        Me.txtCommunity.Name = "txtCommunity"
        Me.txtCommunity.Size = New System.Drawing.Size(148, 20)
        Me.txtCommunity.TabIndex = 3
        '
        'dgvOutput
        '
        Me.dgvOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvOutput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOutput.Location = New System.Drawing.Point(16, 70)
        Me.dgvOutput.Name = "dgvOutput"
        Me.dgvOutput.Size = New System.Drawing.Size(551, 253)
        Me.dgvOutput.TabIndex = 23
        '
        'btnEditComment
        '
        Me.btnEditComment.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditComment.Font = New System.Drawing.Font("Nirmala UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEditComment.Location = New System.Drawing.Point(492, 329)
        Me.btnEditComment.Name = "btnEditComment"
        Me.btnEditComment.Size = New System.Drawing.Size(75, 24)
        Me.btnEditComment.TabIndex = 4
        Me.btnEditComment.Text = "Comment"
        Me.btnEditComment.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'lblBrokenRings
        '
        Me.lblBrokenRings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblBrokenRings.AutoSize = True
        Me.lblBrokenRings.Font = New System.Drawing.Font("Nirmala UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblBrokenRings.Location = New System.Drawing.Point(15, 334)
        Me.lblBrokenRings.Name = "lblBrokenRings"
        Me.lblBrokenRings.Size = New System.Drawing.Size(105, 19)
        Me.lblBrokenRings.TabIndex = 24
        Me.lblBrokenRings.Text = "Broken Rings: "
        '
        'lblConfigPath
        '
        Me.lblConfigPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblConfigPath.AutoSize = True
        Me.lblConfigPath.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblConfigPath.Location = New System.Drawing.Point(16, 403)
        Me.lblConfigPath.Name = "lblConfigPath"
        Me.lblConfigPath.Size = New System.Drawing.Size(71, 15)
        Me.lblConfigPath.TabIndex = 25
        Me.lblConfigPath.Text = "Config Path"
        Me.lblConfigPath.Visible = False
        '
        'lblSortBy
        '
        Me.lblSortBy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSortBy.AutoSize = True
        Me.lblSortBy.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblSortBy.Location = New System.Drawing.Point(389, 390)
        Me.lblSortBy.Name = "lblSortBy"
        Me.lblSortBy.Size = New System.Drawing.Size(51, 15)
        Me.lblSortBy.TabIndex = 26
        Me.lblSortBy.Text = "Sort By:"
        '
        'cboSort
        '
        Me.cboSort.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSort.FormattingEnabled = True
        Me.cboSort.Items.AddRange(New Object() {"Ring ID", "Hostname", "Ring Status"})
        Me.cboSort.Location = New System.Drawing.Point(446, 388)
        Me.cboSort.Name = "cboSort"
        Me.cboSort.Size = New System.Drawing.Size(121, 21)
        Me.cboSort.TabIndex = 2
        '
        'rdoREP
        '
        Me.rdoREP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rdoREP.Location = New System.Drawing.Point(432, 362)
        Me.rdoREP.Name = "rdoREP"
        Me.rdoREP.Size = New System.Drawing.Size(47, 17)
        Me.rdoREP.TabIndex = 27
        Me.rdoREP.Text = "REP"
        Me.rdoREP.UseVisualStyleBackColor = True
        '
        'rdoRAPS
        '
        Me.rdoRAPS.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rdoRAPS.Checked = True
        Me.rdoRAPS.Location = New System.Drawing.Point(432, 333)
        Me.rdoRAPS.Name = "rdoRAPS"
        Me.rdoRAPS.Size = New System.Drawing.Size(54, 17)
        Me.rdoRAPS.TabIndex = 27
        Me.rdoRAPS.TabStop = True
        Me.rdoRAPS.Text = "RAPS"
        Me.rdoRAPS.UseVisualStyleBackColor = True
        '
        'lblLastUpdated
        '
        Me.lblLastUpdated.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblLastUpdated.AutoSize = True
        Me.lblLastUpdated.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblLastUpdated.Location = New System.Drawing.Point(389, 415)
        Me.lblLastUpdated.Name = "lblLastUpdated"
        Me.lblLastUpdated.Size = New System.Drawing.Size(86, 15)
        Me.lblLastUpdated.TabIndex = 25
        Me.lblLastUpdated.Text = "Last Updated: "
        Me.lblLastUpdated.Visible = False
        '
        'lblTotalRings
        '
        Me.lblTotalRings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalRings.AutoSize = True
        Me.lblTotalRings.Font = New System.Drawing.Font("Nirmala UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalRings.Location = New System.Drawing.Point(181, 334)
        Me.lblTotalRings.Name = "lblTotalRings"
        Me.lblTotalRings.Size = New System.Drawing.Size(86, 19)
        Me.lblTotalRings.TabIndex = 24
        Me.lblTotalRings.Text = "Total Rings:"
        Me.lblTotalRings.Visible = False
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(579, 439)
        Me.Controls.Add(Me.rdoRAPS)
        Me.Controls.Add(Me.rdoREP)
        Me.Controls.Add(Me.cboSort)
        Me.Controls.Add(Me.lblSortBy)
        Me.Controls.Add(Me.lblLastUpdated)
        Me.Controls.Add(Me.lblConfigPath)
        Me.Controls.Add(Me.lblTotalRings)
        Me.Controls.Add(Me.lblBrokenRings)
        Me.Controls.Add(Me.dgvOutput)
        Me.Controls.Add(Me.txtCommunity)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.btnEditComment)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "Ring Mon"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.dgvOutput, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents QuitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents NodesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtCommunity As System.Windows.Forms.TextBox
    Friend WithEvents dgvOutput As System.Windows.Forms.DataGridView
    Friend WithEvents btnEditComment As System.Windows.Forms.Button
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblBrokenRings As System.Windows.Forms.Label
    Friend WithEvents lblConfigPath As System.Windows.Forms.Label
    Friend WithEvents lblSortBy As System.Windows.Forms.Label
    Friend WithEvents cboSort As System.Windows.Forms.ComboBox
    Friend WithEvents rdoREP As System.Windows.Forms.RadioButton
    Friend WithEvents rdoRAPS As System.Windows.Forms.RadioButton
    Friend WithEvents ConfigToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblLastUpdated As System.Windows.Forms.Label
    Friend WithEvents lblTotalRings As System.Windows.Forms.Label

End Class
