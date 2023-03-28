using Halcyon.Clients;
using HalcyonManagement.Entities;
using HalcyonSoft.SharedEntities;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace HalcyonManager.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;
        public HomeViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;

                var name = "Welcome!";
                Title = name;
            
        }

        public async void OnAppearing()
        {
            try
            {
                IsBusy = true;
                LineGraphTitle = $"Total Completed Work for {DateTime.Now.Year}";
                PieGraphTitle = $"Work Tasks Overview for {DateTime.Now.Year}";
                BarGraphTitle = $"Comparison of Last Month & Current Month";
                DashBoardData = await _transactionServices.GetDashBoardData(DeviceInfo.Name.RemoveSpecialCharacters());

                DashBoardData.percentageData.percentCompleted = DashBoardData.percentageData.percentCompleted.ToString() == "NaN" ? 0 : DashBoardData.percentageData.percentCompleted;
                DashBoardData.percentageData.percentUnCompleted = DashBoardData.percentageData.percentUnCompleted.ToString() == "NaN" ? 100 : DashBoardData.percentageData.percentUnCompleted;

                //   var data = new double[] { Percentages.PercentUnCompleted, Percentages.PercentCompleted };

                var graphData = new double[] { DashBoardData.percentageData.percentUnCompleted, DashBoardData.percentageData.percentCompleted };
                BarSeries = new ISeries[]
                   {
           new ColumnSeries<int>
        {
            Name = DashBoardData.barGraphData.LastMonth,
            Values = new [] { DashBoardData.barGraphData.CompletedCountForLastMonth},
           Fill = new SolidColorPaint(SKColors.Blue),


        },
            new ColumnSeries<int>
        {
            Name = DashBoardData.barGraphData.CurrentMonth,
            Values = new [] { DashBoardData.barGraphData.CompletedCountForCurrentMonth },
            Fill = new SolidColorPaint(SKColors.LightBlue)

        }
                   };

                BarXAxes = new List<Axis>
            {
                new Axis
                {
                       TextSize = 0,

                }

            };

                BarYAxes = new List<Axis>
            {
                new Axis
                {
                     TextSize = 35
                }

            };

                var data = new double[] { DashBoardData.percentageData.percentUnCompleted, DashBoardData.percentageData.percentCompleted };

                int counter = 1;
                Series = data.AsLiveChartsPieSeries((value, series) =>
                {
                    if (counter == 1)
                    {
                        series.Name = $"Uncompleted Work Tasks: {value}%";
                        series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                        series.Fill = new SolidColorPaint(SKColors.Yellow);
                        series.DataLabelsSize = 0;
                        series.TooltipLabelFormatter = p => $"{p.PrimaryValue} / {p.StackedValue!.Total} ({p.StackedValue.Share:P2})";
                    }
                    else if (counter == 2)
                    {
                        series.Name = $"Completed Work Tasks: {value}%";
                        series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                        series.Fill = new SolidColorPaint(SKColors.Green);
                        series.DataLabelsSize = 0;
                        series.TooltipLabelFormatter = p => $"{p.PrimaryValue} / {p.StackedValue!.Total} ({p.StackedValue.Share:P2})";
                    }
                    counter++;
                });

                List<string> labels = new List<string>();

                foreach (var item in DashBoardData.lineGraphModel)
                {
                    labels.Add(item.Name);
                };

                List<LineGraphModelItem> lineGraphModels = new List<LineGraphModelItem>();

                SeriesCollection = new ISeries[]
    {
               new LineSeries<LineGraphModelItem>
            {
                Values = new List<LineGraphModelItem>(lineGraphModels = DashBoardData.lineGraphModel),
                Fill = new SolidColorPaint(SKColors.LightSkyBlue),
                Name = $"Total Completed Work",
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 6 },
                GeometryStroke = new SolidColorPaint(SKColors.DarkBlue){ StrokeThickness = 6 },
                TooltipLabelFormatter =
        (chartPoint) => $"{chartPoint.Model.Name}: {chartPoint.Model.TotalCompleted}"
            }
                 };

                XAxes = new List<Axis>
                 {
                     new Axis
                     {
                         Labels = labels,
                         TextSize = 35
                     }
                 };

                YAxes = new List<Axis>
                 {
                     new Axis
                     {
                         TextSize = 35
                     }
                 };

            }
            catch (Exception ex)
            {
                App._alertSvc.ShowConfirmation("Error", $"{ex.Message}", (result =>
                {
                    App._alertSvc.ShowAlert("Result", $"{result}");
                }));
            }
        }



        private DashBoard _dashboard;
        public DashBoard DashBoardData
        {
            get => _dashboard;
            set => SetProperty(ref _dashboard, value);
        }

        private string _lineGraphTitle;
        public string LineGraphTitle
        {
            get => _lineGraphTitle;
            set => SetProperty(ref _lineGraphTitle, value);
        }

        private string _pieGraphTitle;
        public string PieGraphTitle
        {
            get => _pieGraphTitle;
            set => SetProperty(ref _pieGraphTitle, value);
        }

        private string _barGraphTitle;
        public string BarGraphTitle
        {
            get => _barGraphTitle;
            set => SetProperty(ref _barGraphTitle, value);
        }

        private IEnumerable<ISeries> _series;
        public IEnumerable<ISeries> Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        private IEnumerable<ISeries> _barSeries;
        public IEnumerable<ISeries> BarSeries
        {
            get => _barSeries;
            set => SetProperty(ref _barSeries, value);
        }

        private IEnumerable<ISeries> _barSeries2;
        public IEnumerable<ISeries> BarSeries2
        {
            get => _barSeries2;
            set => SetProperty(ref _barSeries2, value);
        }



        private ISeries[] _lineSeries;
        public ISeries[] SeriesCollection
        {
            get => _lineSeries;
            set => SetProperty(ref _lineSeries, value);
        }

        private List<Axis> _xaxes;
        public List<Axis> XAxes
        {
            get => _xaxes;
            set => SetProperty(ref _xaxes, value);
        }

        private List<Axis> _barXaxes;
        public List<Axis> BarXAxes
        {
            get => _barXaxes;
            set => SetProperty(ref _barXaxes, value);
        }

        private List<Axis> _barYaxes;
        public List<Axis> BarYAxes
        {
            get => _barYaxes;
            set => SetProperty(ref _barYaxes, value);
        }

        private List<Axis> _barXaxes2;
        public List<Axis> BarXAxes2
        {
            get => _barXaxes2;
            set => SetProperty(ref _barXaxes2, value);
        }

        private List<Axis> _barYaxes2;
        public List<Axis> BarYAxes2
        {
            get => _barYaxes2;
            set => SetProperty(ref _barYaxes2, value);
        }

        private List<Axis> _yaxes;
        public List<Axis> YAxes
        {
            get => _yaxes;
            set => SetProperty(ref _yaxes, value);
        }


        //public IEnumerable<ISeries> Series { get; set; }
    }
}
