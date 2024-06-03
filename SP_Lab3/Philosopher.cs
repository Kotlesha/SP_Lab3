namespace SP_Lab3
{
    internal class Philosopher
    {
        private Thread _thread;
        private bool _end;
        public string Name { get; private set; }
        private readonly Mutex _leftFork;
        private readonly Mutex _rightFork;
        private readonly int _number;

        public Philosopher(int number, Mutex leftFork, Mutex rightFork)
        {
            _number = number;
            _leftFork = leftFork;
            _rightFork = rightFork;
            _end = false;
            Name = $"philosopher{number}";
        }

        public void Start(Form form, ListBox listBox, Panel panel, int philosopherPeriodReflection, int philosopherMealPeriod, int philosopherDelayAfterMeal)
        {
            _thread = new Thread(() =>
                Process(form,
                listBox,
                panel,
                TimeSpan.FromSeconds(philosopherPeriodReflection),
                TimeSpan.FromSeconds(philosopherMealPeriod),
                TimeSpan.FromSeconds(philosopherDelayAfterMeal)
            ));

            _thread.Start();
        }

        public void Process(Form form, ListBox listBox, Panel panel, TimeSpan philosopherPeriodReflection, TimeSpan philosopherMealPeriod, TimeSpan philosopherDelayAfterMeal)
        {
            try
            {
                while (!_end)
                {
                    Think(form, listBox, panel, philosopherPeriodReflection);

                    if (WaitHandle.WaitAll(new[] { _leftFork, _rightFork }))
                    {
                        Eat(form, listBox, panel, philosopherMealPeriod, philosopherDelayAfterMeal);
                        _leftFork.ReleaseMutex();
                        _rightFork.ReleaseMutex();
                    }
                }
            }
            catch (Exception) { }
        }

        private void Think(Form form, ListBox listBox, Panel panel, TimeSpan philosopherPeriodReflection)
        {
            AddData(form, listBox, $"Философ {_number} думает");
            panel.BackColor = SystemColors.Info;
            Thread.Sleep(philosopherPeriodReflection);
        }

        private void Eat(Form form, ListBox listBox, Panel panel, TimeSpan philosopherMealPeriod, TimeSpan philosopherDelayAfterMeal)
        {
            AddData(form, listBox, $"Философ {_number} ест");
            panel.BackColor = Color.Bisque;
            Thread.Sleep(philosopherMealPeriod);
            AddData(form, listBox, $"Философ {_number} покушал");
            panel.BackColor = SystemColors.GradientActiveCaption;
            Thread.Sleep(philosopherDelayAfterMeal);
        }

        private void AddData(Form form, ListBox listBox, string data)
        {
            if (form.IsHandleCreated) form.Invoke(() => listBox.Items.Add(data));
        }

        public void End() => _end = true;

        public void Interrupt() => _thread.Interrupt();
    }
}
