<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Nodes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Nodes))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnAddNode = New System.Windows.Forms.Button()
        Me.txtNodeIP = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnDeleteNode = New System.Windows.Forms.Button()
        Me.lstOutput = New System.Windows.Forms.ListBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Nirmala UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(74, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 45)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Nodes"
        '
        'btnAddNode
        '
        Me.btnAddNode.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnAddNode.Location = New System.Drawing.Point(29, 99)
        Me.btnAddNode.Name = "btnAddNode"
        Me.btnAddNode.Size = New System.Drawing.Size(75, 23)
        Me.btnAddNode.TabIndex = 2
        Me.btnAddNode.Text = "Add Node"
        Me.btnAddNode.UseVisualStyleBackColor = True
        '
        'txtNodeIP
        '
        Me.txtNodeIP.Location = New System.Drawing.Point(108, 63)
        Me.txtNodeIP.Name = "txtNodeIP"
        Me.txtNodeIP.Size = New System.Drawing.Size(124, 20)
        Me.txtNodeIP.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(41, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 15)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "IP Address:"
        '
        'btnDeleteNode
        '
        Me.btnDeleteNode.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnDeleteNode.Location = New System.Drawing.Point(135, 99)
        Me.btnDeleteNode.Name = "btnDeleteNode"
        Me.btnDeleteNode.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteNode.TabIndex = 3
        Me.btnDeleteNode.Text = "Delete Node"
        Me.btnDeleteNode.UseVisualStyleBackColor = True
        '
        'lstOutput
        '
        Me.lstOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstOutput.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lstOutput.FormattingEnabled = True
        Me.lstOutput.ItemHeight = 15
        Me.lstOutput.Location = New System.Drawing.Point(29, 141)
        Me.lstOutput.Name = "lstOutput"
        Me.lstOutput.Size = New System.Drawing.Size(203, 169)
        Me.lstOutput.TabIndex = 6
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Nodes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(268, 326)
        Me.Controls.Add(Me.lstOutput)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtNodeIP)
        Me.Controls.Add(Me.btnDeleteNode)
        Me.Controls.Add(Me.btnAddNode)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Nodes"
        Me.Text = "Nodes"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnAddNode As System.Windows.Forms.Button
    Friend WithEvents txtNodeIP As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnDeleteNode As System.Windows.Forms.Button
    Friend WithEvents lstOutput As System.Windows.Forms.ListBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
