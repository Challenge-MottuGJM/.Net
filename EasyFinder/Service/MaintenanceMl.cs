namespace EasyFinder.Service;

using Microsoft.ML.Data;

public class ManutencaoInput
{
    public float KmDesdeUltimaRevisao { get; set; }
    public float DiasDesdeUltimaRevisao { get; set; }
    public float IncidentesUltimos30d { get; set; }    // quedas/alertas
    public float UsoDiarioMedioHoras { get; set; }
    public float IdadeEmMeses { get; set; }
    public float VelocidadeMedia { get; set; }
    public float TrocasOleoAtrasadas { get; set; }     // 0/1 ou contagem
}

public class ManutencaoPrediction
{
    [ColumnName("PredictedLabel")]
    public bool VaiPrecisarManutencao { get; set; }

    public float Score { get; set; }         // log-odds
    public float Probability { get; set; }   // probabilidade de manutenção
}
