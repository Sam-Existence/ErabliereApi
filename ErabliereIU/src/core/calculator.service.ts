/**
 * Convert a string that is a thent of a number to a base 10 number.
 * Exemple: "1.2" -> "12"
 * Exemple: "1" -> "1"
 * Exemple: "20,1" -> "201"
 * @param value 
 * @returns 
 */
export const convertTenthToNormale = (value?: string) => {
    if (value == null) {
        return "";
    }

    value = value.replace(",", ".");

    if (value.includes(".")) {
        return value.replace(".", "");
    }

    var rtn = (parseInt(value) * 10).toString();

    if (rtn == NaN.toString()) {
        return "";
    }

    return rtn;
}

export const divideByTen = (temperatureThresholdHight: string | undefined) => {
    if (temperatureThresholdHight !== undefined) {
        return (parseInt(temperatureThresholdHight) / 10).toFixed(1);
    }
    return "";
}

export const divideNByTen = (value: number | undefined): number | null => {
    if (value != null) {
        return value / 10;
    }
    return null;
}