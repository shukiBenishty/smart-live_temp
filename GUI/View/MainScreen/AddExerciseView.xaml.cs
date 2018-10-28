using GUI.ViewModel.MainScreen;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.View.MainScreen
{
    /// <summary>
    /// Interaction logic for AddExerciseView.xaml
    /// </summary>
    public partial class AddExerciseView : UserControl
    {
        private Expander theLastExpander;
        private AddExerciseViewModel _addExerciseViewModel;
        public AddExerciseView(AddExerciseViewModel addExerciseViewModel)
        {
            _addExerciseViewModel = addExerciseViewModel;
            InitializeComponent();
            DataContext = _addExerciseViewModel;

        }



        private void Expander_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (theLastExpander == sender as Expander)
            {
                return;
            }
            if (theLastExpander != null)
            {
                theLastExpander.IsExpanded = false;
            }
            theLastExpander = sender as Expander;
            _addExerciseViewModel.AddExerciseItemViewModel = new AddExerciseItemViewModel(((sender as Expander).Tag as string), _addExerciseViewModel.EventAggregator);
        }
    }
}
