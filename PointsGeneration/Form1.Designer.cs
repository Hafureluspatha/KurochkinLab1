namespace PointsGeneration
{
    partial class Form1
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
            this.LinearSeparability = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.filePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.setsPositions = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.multiplicityRadius = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.intersectionPercentage = new System.Windows.Forms.TextBox();
            this.multiplicityDistance = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.radiusDerivation = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pointsCount = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LinearSeparability
            // 
            this.LinearSeparability.FormattingEnabled = true;
            this.LinearSeparability.Items.AddRange(new object[] {
            "Линейно разделимые",
            "Линейно неразделимые"});
            this.LinearSeparability.Location = new System.Drawing.Point(36, 42);
            this.LinearSeparability.Name = "LinearSeparability";
            this.LinearSeparability.Size = new System.Drawing.Size(155, 21);
            this.LinearSeparability.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Линейная разделимость";
            // 
            // GenerateButton
            // 
            this.GenerateButton.Location = new System.Drawing.Point(614, 272);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(118, 22);
            this.GenerateButton.TabIndex = 2;
            this.GenerateButton.Text = "Сгенерировать";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // filePath
            // 
            this.filePath.Location = new System.Drawing.Point(224, 42);
            this.filePath.Name = "filePath";
            this.filePath.Size = new System.Drawing.Size(379, 20);
            this.filePath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Куда сохранить";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Для разделимых";
            // 
            // setsPositions
            // 
            this.setsPositions.FormattingEnabled = true;
            this.setsPositions.Items.AddRange(new object[] {
            "С касаниями",
            "На расстоянии"});
            this.setsPositions.Location = new System.Drawing.Point(224, 174);
            this.setsPositions.Name = "setsPositions";
            this.setsPositions.Size = new System.Drawing.Size(115, 21);
            this.setsPositions.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 241);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Для неразделимых";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(221, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Положение множеств";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(354, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Радиус первого множества";
            // 
            // multiplicityRadius
            // 
            this.multiplicityRadius.Location = new System.Drawing.Point(357, 175);
            this.multiplicityRadius.Name = "multiplicityRadius";
            this.multiplicityRadius.Size = new System.Drawing.Size(131, 20);
            this.multiplicityRadius.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(221, 241);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Процент пересечения";
            // 
            // intersectionPercentage
            // 
            this.intersectionPercentage.Location = new System.Drawing.Point(224, 257);
            this.intersectionPercentage.Name = "intersectionPercentage";
            this.intersectionPercentage.Size = new System.Drawing.Size(116, 20);
            this.intersectionPercentage.TabIndex = 15;
            // 
            // multiplicityDistance
            // 
            this.multiplicityDistance.Location = new System.Drawing.Point(516, 175);
            this.multiplicityDistance.Name = "multiplicityDistance";
            this.multiplicityDistance.Size = new System.Drawing.Size(135, 20);
            this.multiplicityDistance.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(513, 158);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(222, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Среднее расстояние между множествами";
            // 
            // radiusDerivation
            // 
            this.radiusDerivation.Location = new System.Drawing.Point(735, 174);
            this.radiusDerivation.Name = "radiusDerivation";
            this.radiusDerivation.Size = new System.Drawing.Size(135, 20);
            this.radiusDerivation.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(732, 157);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(185, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Рандомная разница радиусов (в %)";
            // 
            // pointsCount
            // 
            this.pointsCount.Location = new System.Drawing.Point(36, 101);
            this.pointsCount.Name = "pointsCount";
            this.pointsCount.Size = new System.Drawing.Size(131, 20);
            this.pointsCount.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(33, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(180, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Количество точек в одном классе";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 339);
            this.Controls.Add(this.pointsCount);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.radiusDerivation);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.multiplicityDistance);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.intersectionPercentage);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.multiplicityRadius);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.setsPositions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.filePath);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LinearSeparability);
            this.Name = "Form1";
            this.Text = "Point Generation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox LinearSeparability;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.TextBox filePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox setsPositions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox multiplicityRadius;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox intersectionPercentage;
        private System.Windows.Forms.TextBox multiplicityDistance;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox radiusDerivation;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox pointsCount;
        private System.Windows.Forms.Label label10;
    }
}

