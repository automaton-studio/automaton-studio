﻿using AntDesign;
using AutoMapper;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.ViewModels
{
    public class SolutionStepsViewModel : ISolutionStepsViewModel, INotifyPropertyChanged
    {
        #region Members

        private readonly IDesignerViewModel designerViewModel;
        private readonly StepFactory stepFactory;
        private readonly IMapper mapper;

        #endregion

        #region Properties

        private IEnumerable<SolutionStep> steps;
        public IEnumerable<SolutionStep> Steps
        {
            get => steps;

            set
            {
                steps = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public SolutionStepsViewModel(
            IDesignerViewModel designerViewModel,
            StepFactory stepFactory,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.designerViewModel = designerViewModel;
            this.stepFactory = stepFactory;
        }

        #region Public Methods

        public void Initialize()
        {
            Steps = stepFactory.GetSteps();
        }

        public void StepDrag(TreeEventArgs<SolutionStep> args)
        {
            var step = args.Node.DataItem;

            if (!step.IsCategory())
            {
                designerViewModel.StepDrag(step);
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}