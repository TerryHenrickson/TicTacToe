using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.ViewModels
{
    using Prism.Events;
    using System.Collections.ObjectModel;
    using TicTacToe.Model;

    public class StatisticsViewModel : BindableBase
    {
        //public ObservableCollection<string> WinHistory { get; set; }
        private long winsByX;
        private long winsByO;
        private long draws;
        private double proportionWinsByX;
        private double proportionWinsByO;
        private double proportionDraws;

        private Queue<Mark> recentResults;

        public long WinsByX
        {
            get
            {
                return this.winsByX;
            }

            set
            {
                this.SetProperty(ref this.winsByX, value);
            }
        }

        public long WinsByO
        {
            get
            {
                return this.winsByO;
            }

            set
            {
                this.SetProperty(ref this.winsByO, value);
            }
        }

        public long Draws
        {
            get
            {
                return this.draws;
            }

            set
            {
                this.SetProperty(ref this.draws, value);
            }
        }

        public double ProportionWinsByX
        {
            get
            {
                return this.proportionWinsByX;
            }

            set
            {
                this.SetProperty(ref this.proportionWinsByX, value);
            }
        }

        public double ProportionWinsByO
        {
            get
            {
                return this.proportionWinsByO;
            }

            set
            {
                this.SetProperty(ref this.proportionWinsByO, value);
            }
        }

        public double ProportionDraws
        {
            get
            {
                return this.proportionDraws;
            }

            set
            {
                this.SetProperty(ref this.proportionDraws, value);
            }
        }

        public StatisticsViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<GameEndEvent>().Subscribe(this.RecordWin);

            //this.WinHistory = new ObservableCollection<string>();
            this.ResetStatisticsCommand = new DelegateCommand(this.ResetStatistics);
            this.ResetStatistics();
        }
    
        public DelegateCommand ResetStatisticsCommand { get; set; }

        private void RecordWin(Mark mark)
        {
            switch (mark)
            {
                //App.Current.Dispatcher.Invoke(() => { this.WinHistory.Add("Player X won"); });

                case Mark.Cross:
                    this.WinsByX += 1;
                    break;
                case Mark.Nought:
                    this.WinsByO += 1;
                    break;
                case Mark.Cross | Mark.Nought:
                    this.Draws += 1;
                    break;
                default:
                    break;
            }
            this.UpdateRecentResults(mark);

        }

        private void UpdateRecentResults(Mark mark)
        {
            while(this.recentResults.Count > 250)
            {
                this.recentResults.Dequeue();
            }

            this.recentResults.Enqueue(mark);

            var size = (double)this.recentResults.Count;
            this.ProportionWinsByX = (Math.Round(10 * (double)this.recentResults.Where(result => (result & Mark.Cross) != 0 && (result & Mark.Nought) == 0).Count() / size))/10;
            this.ProportionWinsByO = (Math.Round(10 * (double)this.recentResults.Where(result => (result & Mark.Cross) == 0 && (result & Mark.Nought) != 0).Count() / size))/10;
            this.ProportionDraws = (Math.Round(10 * (double)this.recentResults.Where(result => ((result & Mark.Cross) != 0 && (result & Mark.Nought) != 0)).Count() / size))/10;


        }


        private void ResetStatistics()
        {
            //this.WinHistory.Clear();
            this.WinsByO = 0;
            this.WinsByX = 0;
            this.Draws = 0;
            this.recentResults = new Queue<Mark>();
            this.ProportionWinsByX = 0;
            this.ProportionWinsByO = 0;
            this.ProportionDraws = 0;

        }
    }
}
