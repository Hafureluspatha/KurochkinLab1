namespace PointsGeneration
{
    partial class PointGeneration
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
            this.label4 = new System.Windows.Forms.Label();
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
            this.label5 = new System.Windows.Forms.Label();
            this.distanceDifference = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dimensions = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.loneClass = new System.Windows.Forms.CheckBox();
            this.allCrossed = new System.Windows.Forms.CheckBox();
            this.allCrossedBar1 = new System.Windows.Forms.CheckBox();
            this.classCount = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
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
            this.GenerateButton.Location = new System.Drawing.Point(784, 254);
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
            this.filePath.Size = new System.Drawing.Size(332, 20);
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
            this.label3.Location = new System.Drawing.Point(72, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Для разделимых";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 241);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Для неразделимых";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(221, 136);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Радиус первого множества";
            // 
            // multiplicityRadius
            // 
            this.multiplicityRadius.Location = new System.Drawing.Point(224, 153);
            this.multiplicityRadius.Name = "multiplicityRadius";
            this.multiplicityRadius.Size = new System.Drawing.Size(135, 20);
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
            this.intersectionPercentage.Size = new System.Drawing.Size(135, 20);
            this.intersectionPercentage.TabIndex = 15;
            // 
            // multiplicityDistance
            // 
            this.multiplicityDistance.Location = new System.Drawing.Point(421, 153);
            this.multiplicityDistance.Name = "multiplicityDistance";
            this.multiplicityDistance.Size = new System.Drawing.Size(135, 20);
            this.multiplicityDistance.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(418, 136);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(222, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Среднее расстояние между множествами";
            // 
            // radiusDerivation
            // 
            this.radiusDerivation.Location = new System.Drawing.Point(224, 193);
            this.radiusDerivation.Name = "radiusDerivation";
            this.radiusDerivation.Size = new System.Drawing.Size(135, 20);
            this.radiusDerivation.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(221, 176);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(191, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Рандомная разница радиусов (в +%)";
            // 
            // pointsCount
            // 
            this.pointsCount.Location = new System.Drawing.Point(36, 101);
            this.pointsCount.Name = "pointsCount";
            this.pointsCount.Size = new System.Drawing.Size(155, 20);
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(418, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(197, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Рандомная разница расстяний (в +%)";
            // 
            // distanceDifference
            // 
            this.distanceDifference.Location = new System.Drawing.Point(421, 193);
            this.distanceDifference.Name = "distanceDifference";
            this.distanceDifference.Size = new System.Drawing.Size(135, 20);
            this.distanceDifference.TabIndex = 22;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(827, 282);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "Показать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dimensions
            // 
            this.dimensions.Location = new System.Drawing.Point(224, 101);
            this.dimensions.Name = "dimensions";
            this.dimensions.Size = new System.Drawing.Size(135, 20);
            this.dimensions.TabIndex = 25;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(221, 84);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Количество измерений";
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(767, 312);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(135, 20);
            this.status.TabIndex = 27;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(720, 315);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "Статус";
            // 
            // loneClass
            // 
            this.loneClass.AutoSize = true;
            this.loneClass.Location = new System.Drawing.Point(639, 153);
            this.loneClass.Name = "loneClass";
            this.loneClass.Size = new System.Drawing.Size(130, 17);
            this.loneClass.TabIndex = 28;
            this.loneClass.Text = "Один класс вдалеке";
            this.loneClass.UseVisualStyleBackColor = true;
            // 
            // allCrossed
            // 
            this.allCrossed.AutoSize = true;
            this.allCrossed.Location = new System.Drawing.Point(383, 257);
            this.allCrossed.Name = "allCrossed";
            this.allCrossed.Size = new System.Drawing.Size(121, 17);
            this.allCrossed.TabIndex = 29;
            this.allCrossed.Text = "Все пересекаются";
            this.allCrossed.UseVisualStyleBackColor = true;
            // 
            // allCrossedBar1
            // 
            this.allCrossedBar1.AutoSize = true;
            this.allCrossedBar1.Location = new System.Drawing.Point(511, 257);
            this.allCrossedBar1.Name = "allCrossedBar1";
            this.allCrossedBar1.Size = new System.Drawing.Size(194, 17);
            this.allCrossedBar1.TabIndex = 30;
            this.allCrossedBar1.Text = "Все пересекаются кроме одного";
            this.allCrossedBar1.UseVisualStyleBackColor = true;
            // 
            // classCount
            // 
            this.classCount.Location = new System.Drawing.Point(582, 41);
            this.classCount.Name = "classCount";
            this.classCount.Size = new System.Drawing.Size(168, 20);
            this.classCount.TabIndex = 31;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(582, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(168, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "Сколько классов генерировать";
            // 
            // PointGeneration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 344);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.classCount);
            this.Controls.Add(this.allCrossedBar1);
            this.Controls.Add(this.allCrossed);
            this.Controls.Add(this.loneClass);
            this.Controls.Add(this.status);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.dimensions);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.distanceDifference);
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
            this.Controls.Add(this.label2);
            this.Controls.Add(this.filePath);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LinearSeparability);
            this.Name = "PointGeneration";
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
        private System.Windows.Forms.Label label4;
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox distanceDifference;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox dimensions;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox status;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox loneClass;
        private System.Windows.Forms.CheckBox allCrossed;
        private System.Windows.Forms.CheckBox allCrossedBar1;
        private System.Windows.Forms.TextBox classCount;
        private System.Windows.Forms.Label label13;
    }
}

