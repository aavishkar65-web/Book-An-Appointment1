using Book_An_Appointment1.AppService;
//using Book_An_Appointment1.AppService.Book_An_Appointment1.AppService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Book_An_Appointment1.Services;

namespace Book_An_Appointment1.Pages
{
    public class BindFacilityModel : PageModel
    {
        private readonly ClientBookAppoints _facilityService;
        private readonly ILogger<BindFacilityModel> _logger;

        public BindFacilityModel(
            ClientBookAppoints facilityService,
            ILogger<BindFacilityModel> logger)
        {
            _facilityService = facilityService;
            _logger = logger;
        }

        public List<SelectListItem> FacilityList { get; set; } = new();

        [BindProperty]
        public string SelectedFacilityId { get; set; }

        public string ErrorMessage { get; set; }

        // ── Page load pe auto run ──────────────────────────────
        public async Task OnGetAsync()
        {
            await LoadFacilitiesAsync();
        }

        // ── Load Data button click pe ──────────────────────────
        public async Task<IActionResult> OnPostAsync()
        {
            await LoadFacilitiesAsync();
            return Page();
        }

        private async Task LoadFacilitiesAsync()
        {
            try
            {
                //string token=new TokenService.t
                // facilityCode null pass karo — sab facilities aayengi
                // Agar specific code chahiye: GetFacilityAsync("YOUR_CODE")
                var result = await _facilityService.GetFacilityAsync();

                if (result?.Data == null || !result.Data.Any())
                {
                    ErrorMessage = "Koi facility nahi mili.";
                    BuildEmptyDropdown();
                    return;
                }

                FacilityList = result.Data
                    .Select(f => new SelectListItem
                    {
                        Value = f.FacilityId.ToString(),
                        Text = f.FacilityName
                    }).ToList();

                FacilityList.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "-- Select Facility --",
                    Selected = true
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Token error");
                ErrorMessage = "Session expire ho gayi. Page refresh karo.";
                BuildEmptyDropdown();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Facility load error");
                ErrorMessage = ex.Message;
                BuildEmptyDropdown();
            }
        }

        private void BuildEmptyDropdown()
        {
            FacilityList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Select Facility --", Selected = true }
            };
        }
    }
}
