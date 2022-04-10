using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages
{
    public partial class CustomNotFound
	{
		[Inject] 
		public NavigationManager NavigationManager { get; set; }
		
		public void NavigateToHome() 
		{ 
			NavigationManager.NavigateTo("/"); 
		}
	}
}
