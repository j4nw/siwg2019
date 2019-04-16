﻿using Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VisualizationWPFApp
{
    public class Model
    {
        public ObservableCollection<ProblemVisualization> ProblemList { get; private set; }
        public ObservableCollection<ProblemVisualization> RecentList { get; set; }
        public ProblemVisualization SelectedProblem { get; set; }
        public ProblemVisualization SelectedRecent { get; set; }

        public Bitmap GetVisualization()
        {
            throw new NotImplementedException();
        }

        public void LoadProblemList()
        {
            ProblemList.Clear();

            // PROBLEM CLASSES LIST
            ProblemList.Add(new ExampleProblemVisualization());
            ProblemList.Add(new ExampleProblemVisualization());
            // ---
            SelectedProblem = ProblemList[0];           
        }

        public Model()
        {
            ProblemList = new ObservableCollection<ProblemVisualization>();
            RecentList = new ObservableCollection<ProblemVisualization>();
            RecentList.Add(new ExampleProblemVisualization());
        }
    }
}