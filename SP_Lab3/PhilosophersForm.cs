namespace SP_Lab3
{
    public partial class PhilosophersForm : Form
    {
        private Philosopher[] philosophers;
        private Mutex[] forks;
        private const int countOfPhilosophers = 6;

        public PhilosophersForm()
        {
            InitializeComponent();
            philosophers = Array.Empty<Philosopher>();
            forks = Array.Empty<Mutex>();
            Stop.Enabled = false;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Information.Items.Clear();
            philosophers = new Philosopher[countOfPhilosophers];
            forks = new Mutex[countOfPhilosophers];

            for (int i = 0; i < forks.Length; i++) forks[i] = new();

            for (int i = 0; i < philosophers.Length; i++)
            {
                Mutex leftFork = forks[i], rightFork = forks[(i + 1) % philosophers.Length];
                philosophers[i] = i == philosophers.Length - 1 ? new(i + 1, rightFork, leftFork) : new(i + 1, leftFork, rightFork);
                var panel = Controls.Find(philosophers[i].Name, true).FirstOrDefault() as Panel;
                philosophers[i].Start(this, Information, panel, (int)philosopherPeriodReflection.Value, (int)philosopherMealPeriod.Value, (int)philosopherDelayAfterMeal.Value);
            }

            Start.Enabled = false;
            Stop.Enabled = true;
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            philosophersInterrupt();
            Start.Enabled = true;
            Stop.Enabled = false;
        }

        private void PhilosophersForm_FormClosing(object sender, FormClosingEventArgs e) => philosophersInterrupt();

        private void philosophersInterrupt()
        {
            foreach (var philosopher in philosophers) philosopher.Interrupt();
        }
    }
}