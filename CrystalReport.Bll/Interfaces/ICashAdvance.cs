using CrystalReport.Bll.Models;

namespace CrystalReport.Bll.Interfaces
{
    public interface ICashAdvance
    {
        void SubmitReport(CashAdvanceModel model);
    }
}
