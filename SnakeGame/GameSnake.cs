using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class SnakeGame : Form
    {

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = 0x20000;
                return cp;
            }
        }

        private static Point point = new Point(0, 0);
        private static bool boolean = false, booleanNotDobule = true;
        private static Random rand = new Random();
        private static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private static string ConditionToKeyBoard = "rightFlow";
        private static int[] arrayJarOfCounts = new int[] { 2, 22, 2, 0, 0}, BottomDetect = new int[22];
        private static int numberCountForMinus = 0, countingEaten = 1, handlingRandom = 0;
        private static List<int> NumberFlowMius = new List<int>();

        public SnakeGame()
        {
            InitializeComponent();
            Exit.Click += (object control, EventArgs e) => this.Close();

            TopChangePosition.MouseDown += (object control, MouseEventArgs e) => {
                point = new Point(e.X, e.Y);
                boolean = true;
            };

            TopChangePosition.MouseUp += (object control, MouseEventArgs e) => boolean = false;
            TopChangePosition.MouseMove += (object control, System.Windows.Forms.MouseEventArgs e) => {
                if (boolean != false)
                {
                    Point points = PointToScreen(e.Location);
                    this.Location = new Point(points.X - point.X, points.Y - point.Y);
                }
            };

            PlayAgain.Click += new System.EventHandler((object control, EventArgs e) => this.functionToAll());
            timer.Interval = 50;
            timer.Tick += new System.EventHandler((object controls, EventArgs es) => tickToe(controls, es));    
            button1.Click += new System.EventHandler((object control, EventArgs e) => this.functionToAll());
        }

        private async void functionToAll() {
            ShowGameOverPanel.Visible = false;
            JarOfPanels.Visible = true;
            label3.Visible = true;
            EatenFlow.Visible = true;
            button1.Visible = false;
            JarOfPanels.Controls.Clear();
            numberCountForMinus = 0; countingEaten = 1; handlingRandom = 0;
            ConditionToKeyBoard = "rightFlow";
            arrayJarOfCounts = new int[] { 2, 22, 2, 0, 0 };
            NumberFlowMius = new List<int>();
            BottomDetect = new int[22];
            boolean = false; booleanNotDobule = true;
            EatenFlow.Text = 1.ToString();


            bool bsCon = true;
            List<Task> taskList = new List<Task>();
            int numberCountLeft = 3, numberCountTops = 3, numberCountForTop = 0, numberCountForDetectDown = 0;
            for (int numberCountMakePanel = 1; numberCountMakePanel <= ((572)); numberCountMakePanel++)
            {
                if (numberCountMakePanel <= 550)
                {
                    numberCountForTop++;
                    if (numberCountForTop > 21)
                    {
                        if (bsCon != true)
                        {
                            numberCountTops += 11;
                            numberCountLeft = 3;
                            numberCountForTop = 0;
                        }
                        else
                        {
                            bsCon = false;
                        }
                    }

                    taskList.Add(CreatePanel(numberCountMakePanel, numberCountLeft, numberCountTops));

                    numberCountLeft += 12;

                    if (numberCountMakePanel + 1 > 550)
                    {
                        await Task.WhenAll(taskList);

                        bool bolsFirst = true;
                        foreach (Panel pan in JarOfPanels.Controls)
                        {
                            if (Convert.ToInt32(pan.Tag) >= 2 + 22 && 2 + 22 >= Convert.ToInt32(pan.Tag))
                            {
                                NumberFlowMius.Add((int)pan.Tag);
                                pan.BackColor = System.Drawing.Color.Black;
                                if (bolsFirst)
                                {
                                    handlingRandom = rand.Next(1, 550);
                                    foreach (Panel pans in JarOfPanels.Controls)
                                    {
                                        if (Convert.ToInt32(pans.Tag) == handlingRandom)
                                        {
                                            pans.BackColor = System.Drawing.Color.Black;
                                        }
                                    }
                                    bolsFirst = false;
                                }
                            }
                        }
                        timer.Start();
                    }
                }
                else {
                    BottomDetect[numberCountForDetectDown] = numberCountMakePanel;
                    numberCountForDetectDown++;
                }
            }


            Task CreatePanel(int numberCountMakePanel, int numberCountLefts, int numberCountTop)
            {
                Panel pan = new Panel
                {
                    Name = numberCountMakePanel.ToString(),
                    Tag = numberCountMakePanel,
                    Size = new Size(12, 11),
                    Location = new Point(numberCountLefts, numberCountTop),
                    BackColor = System.Drawing.Color.White
                };

                JarOfPanels.Controls.Add(pan);
                return Task.CompletedTask;
            }
        }


        private void tickToe(object control, EventArgs e) {
            int[] arr = NumberFlowMius.ToArray();

            //ADD..............................................
            switch (ConditionToKeyBoard) {
                case "rightFlow":
                    bool conditionRight = true;
                    arrayJarOfCounts[2] += 1;

                    for (int numerCountForSee = 0; numerCountForSee < arr.Length; numerCountForSee++)
                    {
                        if (arr[numerCountForSee] != 0)
                        {
                            if ((arrayJarOfCounts[2]+arrayJarOfCounts[1]) == arr[numerCountForSee])
                            {
                                conditionRight = false;
                            }
                        }

                        if ((numerCountForSee + 2) > arr.Length) {
                            if (conditionRight)
                            {
                                NumberFlowMius.Add(arrayJarOfCounts[2] + arrayJarOfCounts[1]);
                                foreach (Panel pan in JarOfPanels.Controls)
                                {
                                    if ((arrayJarOfCounts[2] + arrayJarOfCounts[1]) == (Int32)pan.Tag)
                                    {
                                        pan.BackColor = System.Drawing.Color.Black;
                                        if (handlingRandom == (arrayJarOfCounts[2] + arrayJarOfCounts[1]))
                                        {
                                            foreach (Control con in JarOfPanels.Controls)
                                            {
                                                if (con.GetType() == typeof(Panel))
                                                {
                                                    if ((Int32)con.Tag == ((arrayJarOfCounts[2] + arrayJarOfCounts[1]) + 1))
                                                    {
                                                        pan.BackColor = System.Drawing.Color.Black;
                                                        handlingRandom = rand.Next(1, 500);
                                                        Panel RandomPanel = (Panel)(JarOfPanels.Controls[handlingRandom.ToString()]);
                                                        RandomPanel.BackColor = System.Drawing.Color.Black;
                                                        countingEaten++;
                                                        numberCountForMinus--;
                                                        EatenFlow.Text = countingEaten.ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else {
                                timer.Stop();
                                ShowNumberEaten.Text = countingEaten.ToString();
                                ShowGameOverPanel.Visible = true;
                                JarOfPanels.Visible = false;
                                label3.Visible = false;
                                EatenFlow.Visible = false;
                            }
                        }
                    }
                    break;
                case "downFlow":
                    bool conditionToFlow = true;
                    foreach (Panel pan in JarOfPanels.Controls) {
                        if (conditionToFlow == true) {
                            if (arrayJarOfCounts[3] == (Int32)pan.Tag)
                            {
                                bool conditionToDown = true, conditionToDectect = true;
                                NumberFlowMius.Add(arrayJarOfCounts[3]);

                                for (int numberCountForDectect = 0; BottomDetect.Length > numberCountForDectect; numberCountForDectect++)
                                {

                                    if (arrayJarOfCounts[3] >= BottomDetect[numberCountForDectect])
                                    {
                                        conditionToDectect = false;
                                    }

                                    if (numberCountForDectect >= BottomDetect.Length-1) {
                                        if (conditionToDectect)
                                        {
                                            for (int numerCountForSee = 0; numerCountForSee < arr.Length; numerCountForSee++)
                                            {
                                                if (arr[numerCountForSee] != 0)
                                                {
                                                    if (arrayJarOfCounts[3] == arr[numerCountForSee])
                                                    {
                                                        conditionToDown = false;
                                                    }
                                                }

                                                if (numerCountForSee == arr.Length - 1)
                                                {
                                                    if (conditionToDown)
                                                    {
                                                        pan.BackColor = System.Drawing.Color.Black;
                                                        if (handlingRandom == arrayJarOfCounts[3])
                                                        {
                                                            countingEaten++;
                                                            EatenFlow.Text = countingEaten.ToString();
                                                            handlingRandom = rand.Next(1, 500) + 1;
                                                            numberCountForMinus--;
                                                            Thread th = new Thread(() =>
                                                            {

                                                                Panel pan4 = (Panel)(JarOfPanels.Controls[handlingRandom.ToString()]);
                                                                Action ac = () => pan4.BackColor = Color.Black;
                                                                JarOfPanels.BeginInvoke(ac);
                                                            });
                                                            th.Start();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        timer.Stop();
                                                        ShowNumberEaten.Text = countingEaten.ToString();
                                                        ShowGameOverPanel.Visible = true;
                                                        JarOfPanels.Visible = false;
                                                        label3.Visible = false;
                                                        EatenFlow.Visible = false;
                                                    }
                                                }
                                            }
                                            conditionToFlow = false;
                                        }
                                        else {
                                            timer.Stop();
                                            ShowNumberEaten.Text = countingEaten.ToString();
                                            ShowGameOverPanel.Visible = true;
                                            JarOfPanels.Visible = false;
                                            label3.Visible = false;
                                            EatenFlow.Visible = false;
                                        }
                                    }
                                }
                                arrayJarOfCounts[3] += 22;
                            }
                        }
                    }
                    break;
                case "upFlow":
                    bool conditionToFlows = true;
                    foreach (Panel pan in JarOfPanels.Controls)
                    {
                        if (conditionToFlows == true)
                        {
                            if (arrayJarOfCounts[3] == (Int32)pan.Tag)
                            {
                                bool conditionUp = true;
                                NumberFlowMius.Add(arrayJarOfCounts[3]);
                                for (int numerCountForSee = 0; numerCountForSee < arr.Length;numerCountForSee++) {

                                    if (arr[numerCountForSee] != 0) {
                                        if (arrayJarOfCounts[3] == arr[numerCountForSee]) {
                                            conditionUp = false;
                                        }
                                    }

                                    if (arr.Length-1 <= numerCountForSee) {
                                        if (conditionUp)
                                        {
                                            pan.BackColor = System.Drawing.Color.Black;
                                            if (handlingRandom == arrayJarOfCounts[3])
                                            {
                                                countingEaten++;
                                                EatenFlow.Text = countingEaten.ToString();
                                                handlingRandom = rand.Next(1, 500) + 1;
                                                numberCountForMinus--;
                                                Thread th = new Thread(() =>
                                                {

                                                    Panel pan4 = (Panel)(JarOfPanels.Controls[handlingRandom.ToString()]);
                                                    Action ac = () => pan4.BackColor = Color.Black;
                                                    JarOfPanels.BeginInvoke(ac);
                                                });
                                                th.Start();
                                            }
                                        }
                                        else {
                                            timer.Stop();
                                            ShowNumberEaten.Text = countingEaten.ToString();
                                            ShowGameOverPanel.Visible = true;
                                            JarOfPanels.Visible = false;
                                            label3.Visible = false;
                                            EatenFlow.Visible = false;
                                        }
                                    }
                                }
                                arrayJarOfCounts[3] -= 22;
                                conditionToFlows = false;
                            }
                        }
                    }
                    break;
                case "leftFlow":
                    arrayJarOfCounts[4]--;
                    foreach (Panel pan in JarOfPanels.Controls)
                    {
                        if ((arrayJarOfCounts[4] + arrayJarOfCounts[1]) == Convert.ToInt32(pan.Tag))
                        {
                            bool conditionLeft = true;
                            for (int numerCountForSee = 0; numerCountForSee < arr.Length; numerCountForSee++)
                            {
                                if (arr[numerCountForSee] != 0)
                                {
                                    if ((arrayJarOfCounts[4] + arrayJarOfCounts[1]) == arr[numerCountForSee])
                                    {
                                        conditionLeft = false;
                                    }
                                }

                                if (arr.Length - 1 <= numerCountForSee) {
                                    if (conditionLeft)
                                    {
                                        Thread th = new Thread(() =>
                                        {
                                            Action ac = () =>
                                            {
                                                pan.BackColor = System.Drawing.Color.Black;
                                                NumberFlowMius.Add(arrayJarOfCounts[4] + arrayJarOfCounts[1]);
                                                if (handlingRandom == Convert.ToInt32(pan.Tag))
                                                {
                                                    numberCountForMinus--;
                                                    handlingRandom = rand.Next(1, 500);
                                                    Panel pan2 = (Panel)(JarOfPanels.Controls[handlingRandom.ToString()]);
                                                    pan2.BackColor = Color.Black;
                                                    countingEaten++;
                                                    EatenFlow.Text = countingEaten.ToString();
                                                }
                                            };
                                            JarOfPanels.BeginInvoke(ac);
                                        });
                                        th.Start();
                                    }
                                    else {
                                        timer.Stop();
                                        ShowNumberEaten.Text = countingEaten.ToString();
                                        ShowGameOverPanel.Visible = true;
                                        JarOfPanels.Visible = false;
                                        label3.Visible = false;
                                        EatenFlow.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

            bool conditionCatch = true;
            foreach (Panel panErase in JarOfPanels.Controls) {
                if (conditionCatch) {
                    try {
                        if (Convert.ToInt32(panErase.Tag) == NumberFlowMius[numberCountForMinus]) {
                            NumberFlowMius[numberCountForMinus] = 0;
                            panErase.BackColor = System.Drawing.Color.White;
                        }
                    }
                    catch (Exception es)
                    {
                        String err = es.ToString();
                        timer.Stop();
                        ShowNumberEaten.Text = countingEaten.ToString();
                        conditionCatch = false;
                        ShowGameOverPanel.Visible = true;
                        JarOfPanels.Visible = false;
                        label3.Visible = false;
                        EatenFlow.Visible = false;
                    }
                }
            }
            numberCountForMinus++;
        }

        private void OnKeyDowns(object control, KeyEventArgs e)
        {
            if (e.KeyData.ToString() == "S")
            {
                if (booleanNotDobule) {
                    switch (ConditionToKeyBoard) {
                        case "rightFlow":
                            arrayJarOfCounts[3] = arrayJarOfCounts[2] + (arrayJarOfCounts[1] + arrayJarOfCounts[1]);
                            break;
                        case "leftFlow":
                            arrayJarOfCounts[3] = arrayJarOfCounts[4] + (arrayJarOfCounts[1] + arrayJarOfCounts[1]);
                            break;
                    }
                    booleanNotDobule = false;
                    ConditionToKeyBoard = "downFlow";
                }
            }else if (e.KeyData.ToString() == "W") {
                if (booleanNotDobule)
                {
                    switch (ConditionToKeyBoard)
                    {
                        case "rightFlow":
                            arrayJarOfCounts[3] = arrayJarOfCounts[2] - (arrayJarOfCounts[1] - arrayJarOfCounts[1]);
                            break;
                        case "leftFlow":
                            arrayJarOfCounts[3] = arrayJarOfCounts[4] - (arrayJarOfCounts[1] - arrayJarOfCounts[1]);
                            break;
                    }
                    booleanNotDobule = false;
                    ConditionToKeyBoard = "upFlow";
                }
            }
            else if (e.KeyData.ToString() == "D") {
                if (!booleanNotDobule)
                {
                    booleanNotDobule = true;
                    arrayJarOfCounts[2] = arrayJarOfCounts[3] - 23;
                    ConditionToKeyBoard = "rightFlow";
                }
            } else if (e.KeyData.ToString() == "A") {
                if (!booleanNotDobule)
                {
                    booleanNotDobule = true;
                    arrayJarOfCounts[4] = arrayJarOfCounts[3] - 21;
                    ConditionToKeyBoard = "leftFlow";
                }
            }
        }
    }
}
