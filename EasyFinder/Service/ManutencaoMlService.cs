using EasyFinder.Service;
using Microsoft.ML;

public static class ManutencaoMlService
{
    private static readonly object _lock = new();
    private static PredictionEngine<ManutencaoInput, ManutencaoPrediction>? _engine;

    public static PredictionEngine<ManutencaoInput, ManutencaoPrediction> GetOrCreate()
    {
        if (_engine is not null) return _engine;

        lock (_lock)
        {
            if (_engine is not null) return _engine;

            var ml = new MLContext(seed: 42);

            // Amostras exemplificativas: ajuste conforme sua realidade
            var exemplos = new List<(ManutencaoInput x, bool label)>
            {
                (new ManutencaoInput { KmDesdeUltimaRevisao=800,  DiasDesdeUltimaRevisao=20, IncidentesUltimos30d=0, UsoDiarioMedioHoras=2,  IdadeEmMeses=8,  VelocidadeMedia=35, TrocasOleoAtrasadas=0 }, false),
                (new ManutencaoInput { KmDesdeUltimaRevisao=2500, DiasDesdeUltimaRevisao=90, IncidentesUltimos30d=1, UsoDiarioMedioHoras=6,  IdadeEmMeses=24, VelocidadeMedia=45, TrocasOleoAtrasadas=1 }, true),
                (new ManutencaoInput { KmDesdeUltimaRevisao=4000, DiasDesdeUltimaRevisao=120,IncidentesUltimos30d=2, UsoDiarioMedioHoras=7,  IdadeEmMeses=36, VelocidadeMedia=40, TrocasOleoAtrasadas=1 }, true),
                (new ManutencaoInput { KmDesdeUltimaRevisao=1000, DiasDesdeUltimaRevisao=30, IncidentesUltimos30d=0, UsoDiarioMedioHoras=3,  IdadeEmMeses=10, VelocidadeMedia=30, TrocasOleoAtrasadas=0 }, false),
            };

            var rows = exemplos.Select(s => new
            {
                s.x.KmDesdeUltimaRevisao,
                s.x.DiasDesdeUltimaRevisao,
                s.x.IncidentesUltimos30d,
                s.x.UsoDiarioMedioHoras,
                s.x.IdadeEmMeses,
                s.x.VelocidadeMedia,
                s.x.TrocasOleoAtrasadas,
                Label = s.label
            });

            var data = ml.Data.LoadFromEnumerable(rows);

            var pipeline =
                ml.Transforms.Concatenate("Features",
                    nameof(ManutencaoInput.KmDesdeUltimaRevisao),
                    nameof(ManutencaoInput.DiasDesdeUltimaRevisao),
                    nameof(ManutencaoInput.IncidentesUltimos30d),
                    nameof(ManutencaoInput.UsoDiarioMedioHoras),
                    nameof(ManutencaoInput.IdadeEmMeses),
                    nameof(ManutencaoInput.VelocidadeMedia),
                    nameof(ManutencaoInput.TrocasOleoAtrasadas))
                .Append(ml.BinaryClassification.Trainers.SdcaLogisticRegression());

            var model = pipeline.Fit(data);
            _engine = ml.Model.CreatePredictionEngine<ManutencaoInput, ManutencaoPrediction>(model);
            return _engine!;
        }
    }
}
