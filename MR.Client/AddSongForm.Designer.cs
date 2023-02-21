namespace MR.Client
{
    partial class AddSongForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pAddSong = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cbSongs = new System.Windows.Forms.ComboBox();
            this.pAddSong.SuspendLayout();
            this.SuspendLayout();
            // 
            // pAddSong
            // 
            this.pAddSong.Controls.Add(this.btnAdd);
            this.pAddSong.Controls.Add(this.cbSongs);
            this.pAddSong.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pAddSong.Location = new System.Drawing.Point(0, 85);
            this.pAddSong.Name = "pAddSong";
            this.pAddSong.Size = new System.Drawing.Size(482, 186);
            this.pAddSong.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAdd.Location = new System.Drawing.Point(0, 106);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(482, 80);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Добавить песню";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cbSongs
            // 
            this.cbSongs.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbSongs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSongs.FormattingEnabled = true;
            this.cbSongs.Location = new System.Drawing.Point(0, 0);
            this.cbSongs.Name = "cbSongs";
            this.cbSongs.Size = new System.Drawing.Size(482, 28);
            this.cbSongs.TabIndex = 0;
            // 
            // AddSongForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 271);
            this.Controls.Add(this.pAddSong);
            this.MaximumSize = new System.Drawing.Size(500, 318);
            this.MinimumSize = new System.Drawing.Size(500, 318);
            this.Name = "AddSongForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Welcome to MusicRoom";
            this.pAddSong.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pAddSong;
        private Button btnAdd;
        private ComboBox cbSongs;
    }
}