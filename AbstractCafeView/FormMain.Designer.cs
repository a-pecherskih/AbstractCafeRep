﻿namespace AbstractCafeView
{
    partial class FormMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.журналыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заказчикиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.блюдаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.кухняToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.шефыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пополнитьСкладToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRef = new System.Windows.Forms.Button();
            this.buttonPayChoice = new System.Windows.Forms.Button();
            this.buttonChoiceReady = new System.Windows.Forms.Button();
            this.buttonTakeChoiceInWork = new System.Windows.Forms.Button();
            this.buttonCreateChoice = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.журналыToolStripMenuItem,
            this.пополнитьСкладToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1068, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // журналыToolStripMenuItem
            // 
            this.журналыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.заказчикиToolStripMenuItem,
            this.блюдаToolStripMenuItem,
            this.менюToolStripMenuItem,
            this.кухняToolStripMenuItem,
            this.шефыToolStripMenuItem});
            this.журналыToolStripMenuItem.Name = "журналыToolStripMenuItem";
            this.журналыToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.журналыToolStripMenuItem.Text = "Журналы";
            // 
            // заказчикиToolStripMenuItem
            // 
            this.заказчикиToolStripMenuItem.Name = "заказчикиToolStripMenuItem";
            this.заказчикиToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.заказчикиToolStripMenuItem.Text = "Заказчики";
            this.заказчикиToolStripMenuItem.Click += new System.EventHandler(this.заказчикиToolStripMenuItem_Click);
            // 
            // блюдаToolStripMenuItem
            // 
            this.блюдаToolStripMenuItem.Name = "блюдаToolStripMenuItem";
            this.блюдаToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.блюдаToolStripMenuItem.Text = "Блюда";
            this.блюдаToolStripMenuItem.Click += new System.EventHandler(this.блюдаToolStripMenuItem_Click);
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.менюToolStripMenuItem.Text = "Меню";
            this.менюToolStripMenuItem.Click += new System.EventHandler(this.менюToolStripMenuItem_Click);
            // 
            // кухняToolStripMenuItem
            // 
            this.кухняToolStripMenuItem.Name = "кухняToolStripMenuItem";
            this.кухняToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.кухняToolStripMenuItem.Text = "Кухня";
            this.кухняToolStripMenuItem.Click += new System.EventHandler(this.кухняToolStripMenuItem_Click);
            // 
            // шефыToolStripMenuItem
            // 
            this.шефыToolStripMenuItem.Name = "шефыToolStripMenuItem";
            this.шефыToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.шефыToolStripMenuItem.Text = "Шефы";
            this.шефыToolStripMenuItem.Click += new System.EventHandler(this.шефыToolStripMenuItem_Click);
            // 
            // пополнитьСкладToolStripMenuItem
            // 
            this.пополнитьСкладToolStripMenuItem.Name = "пополнитьСкладToolStripMenuItem";
            this.пополнитьСкладToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.пополнитьСкладToolStripMenuItem.Text = "Пополнить склад";
            this.пополнитьСкладToolStripMenuItem.Click += new System.EventHandler(this.пополнитьСкладToolStripMenuItem_Click);
            // 
            // buttonRef
            // 
            this.buttonRef.Location = new System.Drawing.Point(862, 295);
            this.buttonRef.Name = "buttonRef";
            this.buttonRef.Size = new System.Drawing.Size(187, 26);
            this.buttonRef.TabIndex = 20;
            this.buttonRef.Text = "Обновить список";
            this.buttonRef.UseVisualStyleBackColor = true;
            this.buttonRef.Click += new System.EventHandler(this.buttonRef_Click);
            // 
            // buttonPayChoice
            // 
            this.buttonPayChoice.Location = new System.Drawing.Point(862, 237);
            this.buttonPayChoice.Name = "buttonPayChoice";
            this.buttonPayChoice.Size = new System.Drawing.Size(187, 26);
            this.buttonPayChoice.TabIndex = 19;
            this.buttonPayChoice.Text = "Заказ оплачен";
            this.buttonPayChoice.UseVisualStyleBackColor = true;
            this.buttonPayChoice.Click += new System.EventHandler(this.buttonPayChoice_Click);
            // 
            // buttonChoiceReady
            // 
            this.buttonChoiceReady.Location = new System.Drawing.Point(862, 179);
            this.buttonChoiceReady.Name = "buttonChoiceReady";
            this.buttonChoiceReady.Size = new System.Drawing.Size(187, 26);
            this.buttonChoiceReady.TabIndex = 18;
            this.buttonChoiceReady.Text = "Заказ готов";
            this.buttonChoiceReady.UseVisualStyleBackColor = true;
            this.buttonChoiceReady.Click += new System.EventHandler(this.buttonChoiceReady_Click);
            // 
            // buttonTakeChoiceInWork
            // 
            this.buttonTakeChoiceInWork.Location = new System.Drawing.Point(862, 123);
            this.buttonTakeChoiceInWork.Name = "buttonTakeChoiceInWork";
            this.buttonTakeChoiceInWork.Size = new System.Drawing.Size(187, 26);
            this.buttonTakeChoiceInWork.TabIndex = 17;
            this.buttonTakeChoiceInWork.Text = "Отдать на готовку";
            this.buttonTakeChoiceInWork.UseVisualStyleBackColor = true;
            this.buttonTakeChoiceInWork.Click += new System.EventHandler(this.buttonTakeChoiceInWork_Click);
            // 
            // buttonCreateChoice
            // 
            this.buttonCreateChoice.Location = new System.Drawing.Point(862, 68);
            this.buttonCreateChoice.Name = "buttonCreateChoice";
            this.buttonCreateChoice.Size = new System.Drawing.Size(187, 26);
            this.buttonCreateChoice.TabIndex = 16;
            this.buttonCreateChoice.Text = "Создать заказ";
            this.buttonCreateChoice.UseVisualStyleBackColor = true;
            this.buttonCreateChoice.Click += new System.EventHandler(this.buttonCreateChoice_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(0, 43);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(856, 333);
            this.dataGridView.TabIndex = 15;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 390);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.buttonRef);
            this.Controls.Add(this.buttonPayChoice);
            this.Controls.Add(this.buttonChoiceReady);
            this.Controls.Add(this.buttonTakeChoiceInWork);
            this.Controls.Add(this.buttonCreateChoice);
            this.Controls.Add(this.dataGridView);
            this.Name = "FormMain";
            this.Text = "Главная";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem журналыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заказчикиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem блюдаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem кухняToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem шефыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пополнитьСкладToolStripMenuItem;
        private System.Windows.Forms.Button buttonRef;
        private System.Windows.Forms.Button buttonPayChoice;
        private System.Windows.Forms.Button buttonChoiceReady;
        private System.Windows.Forms.Button buttonTakeChoiceInWork;
        private System.Windows.Forms.Button buttonCreateChoice;
        private System.Windows.Forms.DataGridView dataGridView;
    }
}