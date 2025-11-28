using Manifold.ApplicationLayer.Services;
using Manifold.DomainLayer.Entities;
using Manifold.DomainLayer.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Maninfold.UILayer.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly IList<GraphElement> graph;
        private readonly IManifoldService manifoldService;
        private readonly IRoutingService routingService;
        private readonly IFlowService flowService;

        public ObservableCollection<GraphElementViewModel> Elements { get; } = new();
        public ObservableCollection<PipeViewModel> Pipes { get; } = new();
        public ObservableCollection<GraphElement> Chambers { get; } = new ObservableCollection<GraphElement>();
        public GraphElementViewModel? SelectedChamber { get; set; }

        public ICommand FillCommand { get; }
        public ICommand EmptyCommand { get; }

        public string StatusMessage { get; set; } = "Ready";

        public MainViewModel(IList<GraphElement> graph, IManifoldService manifoldService, IRoutingService routingService, IFlowService flowService)
        {
            this.graph = graph;
            this.manifoldService = manifoldService;
            this.routingService = routingService;
            this.flowService = flowService;

            BuildViewModels();

            FillCommand = new RelayCommand(async p => {
                var type = ParseContent(p?.ToString());
                if (SelectedChamber != null && type != null)
                {
                    manifoldService.FillChamber(SelectedChamber.Id, type.Value);
                    RefreshPipesAndChambers();
                    StatusMessage = $"Filled {SelectedChamber.Id} with {type}";
                    Notify(nameof(StatusMessage));
                }
            });

            EmptyCommand = new RelayCommand(async p => {
                if (SelectedChamber == null) return;
                bool hazardous = (p?.ToString() == "Hw");
                manifoldService.EmptyChamber(SelectedChamber.Id, hazardous);
                RefreshPipesAndChambers();
                StatusMessage = $"Emptied {SelectedChamber.Id} to {(hazardous ? "Hw" : "Nhw")}";
                Notify(nameof(StatusMessage));
            });
        }

        private ContentTypes? ParseContent(string? s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            return s switch
            {
                "Water" => ContentTypes.Water,
                "Alcohol" => ContentTypes.Alcohol,
                _ => null
            };
        }

        private void BuildViewModels()
        {
            // Create element view models and simple layout: sources top, valves middle, chambers right
            var sources = graph.OfType<Manifold.DomainLayer.Entities.SourceValve>().ToList();
            var valves = graph.OfType<Manifold.DomainLayer.Entities.Valve>().ToList();
            var waste = graph.OfType<Manifold.DomainLayer.Entities.WasteValve>().ToList();
            var chambers = graph.OfType<Manifold.DomainLayer.Entities.Chamber>().ToList();

            // Simple grid layout: columns = sources, valves, chambers, waste
            double width = 900, height = 600;
            double colW = width / 4.0;
            double rowStep = 80;

            // Place sources
            for (int i = 0; i < sources.Count; i++)
            {
                var vm = new GraphElementViewModel(sources[i]);
                vm.X = 20;
                vm.Y = 20 + i * rowStep;
                vm.ShortInfo = ((Manifold.DomainLayer.Entities.SourceValve)sources[i]).Content.ToString();
                Elements.Add(vm);
            }

            // Place valves (grouped)
            for (int i = 0; i < valves.Count; i++)
            {
                var vm = new GraphElementViewModel(valves[i]);
                vm.X = colW - 40;
                vm.Y = 20 + i * 48;
                vm.ShortInfo = "Valve";
                Elements.Add(vm);
            }

            // Place chambers on right part
            for (int i = 0; i < chambers.Count; i++)
            {
                var vm = new GraphElementViewModel(chambers[i]);
                vm.X = colW * 2.1;
                vm.Y = 40 + i * 120;
                vm.ShortInfo = "Chamber: " + ((Manifold.DomainLayer.Entities.Chamber)chambers[i]).Content.ToString();
                Elements.Add(vm);
                Chambers.Add(chambers[i]);
            }

            // Place waste valves
            for (int i = 0; i < waste.Count; i++)
            {
                var vm = new GraphElementViewModel(waste[i]);
                vm.X = colW * 3.0;
                vm.Y = 60 + i * 120;
                vm.ShortInfo = ((Manifold.DomainLayer.Entities.WasteValve)waste[i]).IsHazardous ? "Waste: Hw" : "Waste: Nhw";
                Elements.Add(vm);
            }

            // Map pipes to viewmodels and compute basic coordinates by using element positions
            var pipes = graph.OfType<Manifold.DomainLayer.Entities.Pipe>().ToList();
            foreach (var pipe in pipes)
            {
                var pvm = new PipeViewModel(pipe);
                var fromVm = Elements.FirstOrDefault(e => e.Id == pipe.From.Id);
                var toVm = Elements.FirstOrDefault(e => e.Id == pipe.To.Id);
                if (fromVm != null && toVm != null)
                {
                    pvm.X1 = fromVm.X + 40;
                    pvm.Y1 = fromVm.Y + 24;
                    pvm.X2 = toVm.X + 40;
                    pvm.Y2 = toVm.Y + 24;
                }
                Pipes.Add(pvm);
            }

            // Select first chamber by default
            if (Chambers.Count > 0)
            {
                var first = Chambers[0];
                SelectedChamber = Elements.FirstOrDefault(e => e.Id == first.Id);
            }
        }

        private void RefreshPipesAndChambers()
        {
            // Update pipe content bindings (they read from domain pipes)
            foreach (var pvm in Pipes)
            {
                // Force update by raising property changed if needed - simple approach: recreate collection
            }

            // Recreate Pipes collection to refresh UI (simpler than implementing change events on domain)
            Pipes.Clear();
            var pipes = graph.OfType<Manifold.DomainLayer.Entities.Pipe>().ToList();
            foreach (var pipe in pipes)
            {
                var pvm = new PipeViewModel(pipe);
                var fromVm = Elements.FirstOrDefault(e => e.Id == pipe.From.Id);
                var toVm = Elements.FirstOrDefault(e => e.Id == pipe.To.Id);
                if (fromVm != null && toVm != null)
                {
                    pvm.X1 = fromVm.X + 40;
                    pvm.Y1 = fromVm.Y + 24;
                    pvm.X2 = toVm.X + 40;
                    pvm.Y2 = toVm.Y + 24;
                }
                Pipes.Add(pvm);
            }

            // Update chamber labels
            foreach (var elem in Elements)
            {
                if (graph.OfType<Manifold.DomainLayer.Entities.Chamber>().Any(c => c.Id == elem.Id))
                {
                    var chamber = graph.OfType<Manifold.DomainLayer.Entities.Chamber>().First(c => c.Id == elem.Id);
                    elem.ShortInfo = "Chamber: " + chamber.Content.ToString();
                    elem.Element = elem.Element; // no-op to avoid compile error
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify(string p) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));

    }
}
