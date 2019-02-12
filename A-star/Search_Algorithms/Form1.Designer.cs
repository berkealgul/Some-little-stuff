namespace Search_Algorithms
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.start = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.status = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timeL = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.wall = new System.Windows.Forms.RadioButton();
            this.gravel = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(667, 12);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 23);
            this.start.TabIndex = 0;
            this.start.Text = "Başlat";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // canvas
            // 
            this.canvas.Location = new System.Drawing.Point(12, 12);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(630, 630);
            this.canvas.TabIndex = 1;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDown);
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.status.Location = new System.Drawing.Point(663, 141);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(27, 20);
            this.status.TabIndex = 2;
            this.status.Text = "__";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(663, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Sonuç";
            // 
            // timeL
            // 
            this.timeL.AutoSize = true;
            this.timeL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.timeL.Location = new System.Drawing.Point(663, 242);
            this.timeL.Name = "timeL";
            this.timeL.Size = new System.Drawing.Size(27, 20);
            this.timeL.TabIndex = 5;
            this.timeL.Text = "__";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(663, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Çözüm Süresi";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(648, 326);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Bilgi İçin Tıklayın";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(648, 380);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Duvarları Sıfırla";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // wall
            // 
            this.wall.AutoSize = true;
            this.wall.Checked = true;
            this.wall.Location = new System.Drawing.Point(667, 465);
            this.wall.Name = "wall";
            this.wall.Size = new System.Drawing.Size(54, 17);
            this.wall.TabIndex = 8;
            this.wall.TabStop = true;
            this.wall.Text = "Duvar";
            this.wall.UseVisualStyleBackColor = true;
            // 
            // gravel
            // 
            this.gravel.AutoSize = true;
            this.gravel.Location = new System.Drawing.Point(667, 488);
            this.gravel.Name = "gravel";
            this.gravel.Size = new System.Drawing.Size(48, 17);
            this.gravel.TabIndex = 9;
            this.gravel.Text = "Çakıl";
            this.gravel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(648, 442);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Sol tık modu";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(786, 661);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.gravel);
            this.Controls.Add(this.wall);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.timeL);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.status);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.start);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "A-Star Pathfinding";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button start;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label timeL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton wall;
        private System.Windows.Forms.RadioButton gravel;
        private System.Windows.Forms.Label label5;
    }
}

