namespace MR.Client
{
    partial class PlaylistForm
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.редактироватьПлейлистToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьПеснюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьПеснюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.скопироватьIDКомнатыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.закрытьКомнатуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выйтиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flpSong = new System.Windows.Forms.FlowLayoutPanel();
            this.tbCurSong = new System.Windows.Forms.TextBox();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lvPlaylist = new System.Windows.Forms.ListView();
            this.menuStrip.SuspendLayout();
            this.flpSong.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.редактироватьПлейлистToolStripMenuItem,
            this.скопироватьIDКомнатыToolStripMenuItem,
            this.закрытьКомнатуToolStripMenuItem,
            this.выйтиToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(842, 28);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "msAction";
            // 
            // редактироватьПлейлистToolStripMenuItem
            // 
            this.редактироватьПлейлистToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьПеснюToolStripMenuItem,
            this.удалитьПеснюToolStripMenuItem});
            this.редактироватьПлейлистToolStripMenuItem.Name = "редактироватьПлейлистToolStripMenuItem";
            this.редактироватьПлейлистToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.редактироватьПлейлистToolStripMenuItem.Text = "Редактировать плейлист";
            // 
            // добавитьПеснюToolStripMenuItem
            // 
            this.добавитьПеснюToolStripMenuItem.Name = "добавитьПеснюToolStripMenuItem";
            this.добавитьПеснюToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.добавитьПеснюToolStripMenuItem.Text = "Добавить песню";
            this.добавитьПеснюToolStripMenuItem.Click += new System.EventHandler(this.добавитьПеснюToolStripMenuItem_Click);
            // 
            // удалитьПеснюToolStripMenuItem
            // 
            this.удалитьПеснюToolStripMenuItem.Enabled = false;
            this.удалитьПеснюToolStripMenuItem.Name = "удалитьПеснюToolStripMenuItem";
            this.удалитьПеснюToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.удалитьПеснюToolStripMenuItem.Text = "Удалить песню";
            this.удалитьПеснюToolStripMenuItem.Click += new System.EventHandler(this.удалитьПеснюToolStripMenuItem_Click);
            // 
            // скопироватьIDКомнатыToolStripMenuItem
            // 
            this.скопироватьIDКомнатыToolStripMenuItem.Name = "скопироватьIDКомнатыToolStripMenuItem";
            this.скопироватьIDКомнатыToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.скопироватьIDКомнатыToolStripMenuItem.Text = "Скопировать ID комнаты";
            this.скопироватьIDКомнатыToolStripMenuItem.Click += new System.EventHandler(this.скопироватьIDКомнатыToolStripMenuItem_Click);
            // 
            // закрытьКомнатуToolStripMenuItem
            // 
            this.закрытьКомнатуToolStripMenuItem.Name = "закрытьКомнатуToolStripMenuItem";
            this.закрытьКомнатуToolStripMenuItem.Size = new System.Drawing.Size(141, 24);
            this.закрытьКомнатуToolStripMenuItem.Text = "Закрыть комнату";
            this.закрытьКомнатуToolStripMenuItem.Click += new System.EventHandler(this.закрытьКомнатуToolStripMenuItem_Click);
            // 
            // выйтиToolStripMenuItem
            // 
            this.выйтиToolStripMenuItem.Name = "выйтиToolStripMenuItem";
            this.выйтиToolStripMenuItem.Size = new System.Drawing.Size(67, 24);
            this.выйтиToolStripMenuItem.Text = "Выйти";
            this.выйтиToolStripMenuItem.Click += new System.EventHandler(this.выйтиToolStripMenuItem_Click);
            // 
            // flpSong
            // 
            this.flpSong.Controls.Add(this.tbCurSong);
            this.flpSong.Controls.Add(this.btnPrev);
            this.flpSong.Controls.Add(this.btnPlay);
            this.flpSong.Controls.Add(this.btnNext);
            this.flpSong.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpSong.Location = new System.Drawing.Point(0, 406);
            this.flpSong.Name = "flpSong";
            this.flpSong.Size = new System.Drawing.Size(842, 67);
            this.flpSong.TabIndex = 1;
            // 
            // tbCurSong
            // 
            this.tbCurSong.Location = new System.Drawing.Point(3, 3);
            this.tbCurSong.Name = "tbCurSong";
            this.tbCurSong.ReadOnly = true;
            this.tbCurSong.Size = new System.Drawing.Size(590, 27);
            this.tbCurSong.TabIndex = 0;
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(599, 3);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(70, 50);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(675, 3);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(70, 50);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(751, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(70, 50);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lvPlaylist
            // 
            this.lvPlaylist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPlaylist.Location = new System.Drawing.Point(0, 28);
            this.lvPlaylist.MultiSelect = false;
            this.lvPlaylist.Name = "lvPlaylist";
            this.lvPlaylist.Size = new System.Drawing.Size(842, 378);
            this.lvPlaylist.TabIndex = 2;
            this.lvPlaylist.UseCompatibleStateImageBehavior = false;
            this.lvPlaylist.View = System.Windows.Forms.View.List;
            this.lvPlaylist.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvPlaylist_ItemSelectionChanged);
            // 
            // PlaylistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 473);
            this.Controls.Add(this.lvPlaylist);
            this.Controls.Add(this.flpSong);
            this.Controls.Add(this.menuStrip);
            this.MinimumSize = new System.Drawing.Size(860, 520);
            this.Name = "PlaylistForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Welcome to MusicRoom";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PlaylistForm_FormClosed);
            this.Load += new System.EventHandler(this.PlaylistForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.flpSong.ResumeLayout(false);
            this.flpSong.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem редактироватьПлейлистToolStripMenuItem;
        private ToolStripMenuItem добавитьПеснюToolStripMenuItem;
        private ToolStripMenuItem удалитьПеснюToolStripMenuItem;
        private ToolStripMenuItem закрытьКомнатуToolStripMenuItem;
        private ToolStripMenuItem выйтиToolStripMenuItem;
        private FlowLayoutPanel flpSong;
        private TextBox tbCurSong;
        private Button btnPrev;
        private Button btnPlay;
        private Button btnNext;
        private ListView lvPlaylist;
        private ToolStripMenuItem скопироватьIDКомнатыToolStripMenuItem;
    }
}