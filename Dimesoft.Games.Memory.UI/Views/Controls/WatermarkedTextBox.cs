using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Dimesoft.Games.Memory.Views.Controls
{
    public class WatermarkedTextBox : TextBox
    {
        public WatermarkedTextBox()
            : base()
        {
            this.DefaultStyleKey = typeof(WatermarkedTextBox);

            this.TextChanged += (o, e) =>
            {
                if (string.IsNullOrEmpty(Text))
                    IsWaterMarkVisible = true;
                else
                    IsWaterMarkVisible = false;
            };

        }
        
        protected override void OnApplyTemplate()
        {

            if (string.IsNullOrEmpty(Text))
                IsWaterMarkVisible = true;
            else
                IsWaterMarkVisible = false;

            base.OnApplyTemplate();
        }


        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }
        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(string), typeof(WatermarkedTextBox), new PropertyMetadata(string.Empty));



        public bool IsWaterMarkVisible
        {
            get { return (bool)GetValue(IsWaterMarkVisibleProperty); }
            set { SetValue(IsWaterMarkVisibleProperty, value); }
        }
        public static readonly DependencyProperty IsWaterMarkVisibleProperty =
            DependencyProperty.Register("IsWaterMarkVisible", typeof(bool), typeof(WatermarkedTextBox), new PropertyMetadata(false));
    }
}
