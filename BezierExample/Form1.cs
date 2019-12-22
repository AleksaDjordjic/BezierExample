using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BezierExample
{
    public partial class Form1 : Form
    {
        List<PointF> bezierPoints = new List<PointF>();
        PointF? startPoint;
        PointF? endPoint;

        public Form1()
        {
            InitializeComponent();
            UpdateText();

            // Ovo moze preko forme da se uradi ali ja preferiram ovako, ignorisi
            MouseClick += Form1_MouseClick;
            Paint += Form1_Paint;
        }

        void UpdateText()
        {
            // Pomocni text na vrhu forme, ovo ignorisi

            if (startPoint == null)
                CurrentlyPlacingText.Text = "Currently Placing: Start Point";
            else if (endPoint == null)
                CurrentlyPlacingText.Text = "Currently Placing: End Point";
            else CurrentlyPlacingText.Text = $"Currently Placing: Bezier Point #{bezierPoints.Count}";
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (startPoint == null) // Nemamo pocetnu tacku, napravi je
                startPoint = e.Location;
            else if (endPoint == null) // Nemamo krajnu tacku, napravi je
                endPoint = e.Location;
            else
            {
                if (bezierPoints.Count >= 2) // Ako imamo 2 ili vise pointa, obrisemo prvi
                    bezierPoints.RemoveAt(0);

                bezierPoints.Add(e.Location); // Dodajemo point na kraj liste
            }

            // Pomocni text gore levo
            UpdateText();
            // Obrisemo formu i pozovemo From1_Paint
            Invalidate();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            // Ovo je da ocistimo ono sto smo nacrtali, dugme na formi
            startPoint = default(PointF);
            endPoint = default(PointF);
            bezierPoints = new List<PointF>();

            // Obrisemo formu i pozovemo From1_Paint
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            #region Pomocne tacke

            // Plava tacka za pocetak
            if (startPoint != null)
                e.Graphics.FillEllipse(Brushes.Blue, startPoint.Value.X - 5, startPoint.Value.Y - 5, 10, 10);

            // Crvena tacka za kraj
            if (endPoint != null)
                e.Graphics.FillEllipse(Brushes.Red, endPoint.Value.X - 5, endPoint.Value.Y - 5, 10, 10);

            // Narandzasta tacka za svaki point
            if(bezierPoints.Count >= 1)
                foreach (var p in bezierPoints)
                    e.Graphics.FillEllipse(Brushes.Orange, p.X - 2.5f, p.Y - 2.5f, 5, 5);

            #endregion

            // Ako nemamo pocetak, kraj ili nikakve tacke da crtamo, samo se vrati iz funkcije
            if (startPoint == null || endPoint == null || bezierPoints.Count <= 0)
                return;

            // Skupimo sve tacke u jednu listu
            List<PointF> points = new List<PointF>(); // Napravimo novu listu

            points.AddRange(bezierPoints); // Dodamo bezierPoints Listu na nju
            points.Insert(0, (PointF)startPoint); // Dodamo startPoint na pocetak liste
            points.Add((PointF)endPoint); // Dodamo endPoint na kraj liste

            // Napravimo Pen p i nacrtamo sa DrawBeziers
            using (Pen p = new Pen(Brushes.Black, 5))
                e.Graphics.DrawBeziers(p, points.ToArray() /* Pretvaramo Listu u Array sa .ToArray() */);
        }
    }
}
