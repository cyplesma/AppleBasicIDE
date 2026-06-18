using System.Drawing;
using System.Windows.Forms;

namespace AppleBasic_IDE
{
    internal static class PromptDialog
    {
        public static string Show(string title, string prompt, string defaultValue)
        {
            using Form form = new Form
            {
                Text = title,
                ClientSize = new Size(440, 130),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false
            };

            Label label = new Label
            {
                AutoSize = true,
                Location = new Point(12, 15),
                Text = prompt
            };

            TextBox textBox = new TextBox
            {
                Location = new Point(15, 42),
                Size = new Size(410, 27),
                Text = defaultValue ?? ""
            };

            Button okButton = new Button
            {
                DialogResult = DialogResult.OK,
                Location = new Point(269, 86),
                Size = new Size(75, 28),
                Text = "OK"
            };

            Button cancelButton = new Button
            {
                DialogResult = DialogResult.Cancel,
                Location = new Point(350, 86),
                Size = new Size(75, 28),
                Text = "Cancel"
            };

            form.Controls.AddRange(new Control[] { label, textBox, okButton, cancelButton });
            form.AcceptButton = okButton;
            form.CancelButton = cancelButton;

            textBox.SelectAll();
            return form.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }
    }
}
