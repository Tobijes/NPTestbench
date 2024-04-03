

using Microsoft.EntityFrameworkCore;
using NPTestbench.Controllers;
using NPTestbench.Models;

public class SummaryService {

   
    public async Task<Summary> getLastSummary(){
       using(var context = new DataContext()){

        var latestRunMessurements = await context.Measurements.Where(m => m.RunId == context.Runs.Max(e => e.Id)).ToListAsync();
        Summary summary = new Summary(){
            measurements = latestRunMessurements
        };
        return  summary;
        }
    }
}



public class Summary
{
    public List<Measurement> measurements = new ();

}