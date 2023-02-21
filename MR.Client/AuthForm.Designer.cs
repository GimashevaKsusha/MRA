namespace MR.Client
{
    partial class AuthForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pElements = new System.Windows.Forms.Panel();
            this.gbRoomId = new System.Windows.Forms.GroupBox();
            this.btnConnect2 = new System.Windows.Forms.Button();
            this.tbRoomId = new System.Windows.Forms.TextBox();
            this.btnConnect1 = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.pElements.SuspendLayout();
            this.gbRoomId.SuspendLayout();
            this.SuspendLayout();
            // 
            // pElements
            // 
            this.pElements.Controls.Add(this.gbRoomId);
            this.pElements.Controls.Add(this.btnConnect1);
            this.pElements.Controls.Add(this.btnCreate);
            this.pElements.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pElements.Location = new System.Drawing.Point(0, 70);
            this.pElements.Name = "pElements";
            this.pElements.Size = new System.Drawing.Size(570, 333);
            this.pElements.TabIndex = 0;
            // 
            // gbRoomId
            // 
            this.gbRoomId.Controls.Add(this.btnConnect2);
            this.gbRoomId.Controls.Add(this.tbRoomId);
            this.gbRoomId.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbRoomId.Location = new System.Drawing.Point(0, 179);
            this.gbRoomId.Name = "gbRoomId";
            this.gbRoomId.Size = new System.Drawing.Size(570, 154);
            this.gbRoomId.TabIndex = 2;
            this.gbRoomId.TabStop = false;
            this.gbRoomId.Text = "Идентификатор комнаты";
            this.gbRoomId.Visible = false;
            // 
            // btnConnect2
            // 
            this.btnConnect2.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnConnect2.Enabled = false;
            this.btnConnect2.Location = new System.Drawing.Point(3, 50);
            this.btnConnect2.Name = "btnConnect2";
            this.btnConnect2.Size = new System.Drawing.Size(564, 80);
            this.btnConnect2.TabIndex = 1;
            this.btnConnect2.Text = "Подключиться";
            this.btnConnect2.UseVisualStyleBackColor = true;
            this.btnConnect2.Click += new System.EventHandler(this.btnConnect2_Click);
            // 
            // tbRoomId
            // 
            this.tbRoomId.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbRoomId.Location = new System.Drawing.Point(3, 23);
            this.tbRoomId.Name = "tbRoomId";
            this.tbRoomId.Size = new System.Drawing.Size(564, 27);
            this.tbRoomId.TabIndex = 0;
            this.tbRoomId.TextChanged += new System.EventHandler(this.tbRoomId_TextChanged);
            // 
            // btnConnect1
            // 
            this.btnConnect1.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnConnect1.Location = new System.Drawing.Point(0, 80);
            this.btnConnect1.Name = "btnConnect1";
            this.btnConnect1.Size = new System.Drawing.Size(570, 80);
            this.btnConnect1.TabIndex = 1;
            this.btnConnect1.Text = "Подключиться к комнате";
            this.btnConnect1.UseVisualStyleBackColor = true;
            this.btnConnect1.Click += new System.EventHandler(this.btnConnect1_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCreate.Location = new System.Drawing.Point(0, 0);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(570, 80);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Создать новую комнату";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // AuthForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 403);
            this.Controls.Add(this.pElements);
            this.MinimumSize = new System.Drawing.Size(588, 450);
            this.Name = "AuthForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome to MusicRoom";
            this.pElements.ResumeLayout(false);
            this.gbRoomId.ResumeLayout(false);
            this.gbRoomId.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pElements;
        private Button btnConnect1;
        private Button btnCreate;
        private GroupBox gbRoomId;
        private Button btnConnect2;
        private TextBox tbRoomId;
    }
}