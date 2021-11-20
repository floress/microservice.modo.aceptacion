using System.ComponentModel;

namespace Modo.Clients;

public enum TipoCondicionFiscalEnum
{
    [Description("Certificado MiPyME")]
    MIPYME,

    [Description("Régimen Simplificado")]
    RS,

    [Description("Exención")]
    EX
}