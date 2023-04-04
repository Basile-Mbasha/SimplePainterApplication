namespace SimplePainterApplication
{
    public partial class Form1 : Form
    {
        public List<Shape> Shapes = new List<Shape>();
        public List<Shape> RedoStack = new List<Shape>();

        public Form1()
        {
            InitializeComponent();
            strokeColorDialog.Color = Color.Black;
            fillColorDialog.Color = Color.Aquamarine;
            UpdatePanels();

            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
        }

        private void UpdatePanels()
        {
            panel1.BackColor = strokeColorDialog.Color;
            panel2.BackColor = fillColorDialog.Color;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            strokeColorDialog.ShowDialog();
            UpdatePanels();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fillColorDialog.ShowDialog();
            UpdatePanels();
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!isDrawing)
            {
                var point = pictureBox1.PointToScreen(new Point(0, 0));
                var x = MousePosition.X - point.X;
                var y = MousePosition.Y - point.Y;
                Shapes.Add(new Shape()
                {
                    X = x,
                    Y = y,
                    Width = shapeWidthTrackBar.Value,
                    Height = shapeHeightTrackBar.Value,
                    StrokeColor = strokeColorDialog.Color,
                    FillColor = fillColorDialog.Color,
                    Filled = filledCheckBox.Checked,
                    StrokeThickness = strokeWidthTrackBar.Value,
                    Type = ellipseRadioButton.Checked ? ShapeType.Ellipse : ShapeType.Rect,
                });
            }
             pictureBox1.Invalidate();
       }
        
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var shape in Shapes)
            {
                using var pen = new Pen(shape.StrokeColor, shape.StrokeThickness);
                using var brush = new SolidBrush(shape.FillColor);
                if (shape.Type == ShapeType.Ellipse)
                {
                    if (shape.Filled)
                    {
                        e.Graphics.FillEllipse(brush, shape.X, shape.Y, shape.Width, shape.Height);
                    }

                    e.Graphics.DrawEllipse(pen, shape.X, shape.Y, shape.Width, shape.Height);
                }

                if (shape.Type == ShapeType.Rect)
                {
                    if (shape.Filled)
                    {
                        e.Graphics.FillRectangle(brush, shape.X, shape.Y, shape.Width, shape.Height);
                    }

                    e.Graphics.DrawRectangle(pen, shape.X, shape.Y, shape.Width, shape.Height);
                }
            }
        }
        /* New features implementation starts here... */
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                int x = random.Next(pictureBox1.Width);
                int y = random.Next(pictureBox1.Height);

                Shapes.Add(new Shape()
                {
                    X = x,
                    Y = y,
                    Width = shapeWidthTrackBar.Value,
                    Height = shapeHeightTrackBar.Value,
                    StrokeColor = strokeColorDialog.Color,
                    FillColor = fillColorDialog.Color,
                    Filled = filledCheckBox.Checked,
                    StrokeThickness = strokeWidthTrackBar.Value,
                    Type = ellipseRadioButton.Checked ? ShapeType.Ellipse : ShapeType.Rect,
                });
            }
            pictureBox1.Invalidate();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Shapes.Count > 0)
                Shapes.RemoveAt(Shapes.Count - 1);
            pictureBox1.Invalidate();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shapes.Clear();
            pictureBox1.Invalidate();
        }

        private Shape currentShape; // stores current coordinates until mouseButton is released
        private bool isDrawing;    // checks if the drawing process is true and implements shape drawn on the picture box
        private Point startPoint; // stores the starting point of the shape being drawn
        private Point endPoint;  // stores the ending point of the shape being drawn

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;

            // Set the starting point and create a new Shape
            startPoint = e.Location;
            currentShape = new Shape()
            {
                X = e.X,
                Y = e.Y,
                Width = 0,
                Height = 0,
                StrokeColor = strokeColorDialog.Color,
                FillColor = fillColorDialog.Color,
                Filled = filledCheckBox.Checked,
                StrokeThickness = strokeWidthTrackBar.Value,
                Type = ellipseRadioButton.Checked ? ShapeType.Ellipse : ShapeType.Rect,
            };
            isDrawing = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // Check if the mouse button is down and the user is drawing
            if (isDrawing && e.Button == MouseButtons.Left)
            {
                // Update the end point of the shape being drawn
                endPoint = e.Location;

                // Calculate the size of the shape being drawn
                currentShape.Width = endPoint.X - startPoint.X;
                currentShape.Height = endPoint.Y - startPoint.Y;

                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isDrawing)
            {
                Shapes.Add(currentShape);
             
                currentShape = null;
                isDrawing = false;

                pictureBox1.Invalidate();
            }
        }
    }
}