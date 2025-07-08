using Dapper;
using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity.Payment;
using OLCA.Infrastructure.CQS;
using System;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Emoda.PekiYaBen.Business.Handlers.Commands
{
    public class SavePaymentCommandHandler : ICommandHandler<SavePaymentCommand, int>
    {
        public int Execute(SavePaymentCommand command)
        {
            using (IDbConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    int exist = con.QueryFirstOrDefault<int>("SELECT 1 FROM Payment WHERE TransactionId = @TransactionId", command, trans);

                    if(exist>0)
                    {
                        return 0;
                    }

                    Payment payment = new Payment()
                    {
                        UserId = command.UserId,
                        LogDate = DateTime.Now,
                        ProductId = command.ProductId,
                        PurchaseTime = command.PurchaseTime,
                        Signature = command.Signature,
                        Token = command.Token,
                        TransactionId = command.TransactionId,
                    };

                    var id = con.Query<int>("INSERT INTO Payment (UserId, ProductId, PurchaseTime, Signature,Token,TransactionId, LogDate) VALUES(@UserId, @ProductId, @PurchaseTime, @Signature, @Token, @TransactionId, @LogDate); SELECT CAST(SCOPE_IDENTITY() as int)", payment, trans).Single();
                    trans.Commit();
                    return id;
                }
            }
        }
    }
}
