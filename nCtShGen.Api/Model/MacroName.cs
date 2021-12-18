using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace nCtShGen.Api.Model;

public enum MacroName
{
    rootFolderPath,
    currentFolderPath,
    currentFolderName,
    outputFolderPath,

    [DisplayFormat(DataFormatString = "yyyyMMdd")]
    date,
    [DisplayFormat(DataFormatString = "yyyy")]
    year,
    [DisplayFormat(DataFormatString = "MM")]
    month,
    [DisplayFormat(DataFormatString = "dd")]
    day,
    counter,
}