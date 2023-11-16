import { ChartDataset } from "chart.js";

export const calculerMoyenne = (data: ChartDataset) => {
    // calculer la moyenne des valeurs dans data
    // faire la somme
    // diviser par le nombre de valeurs
    // retourner la moyenne
    let mean = 0;

    if (data.data != undefined) {
        data.data.forEach(element => {
            if (typeof element === 'number')
                mean += element;
        });
    }

    mean = mean / data.data?.length as number;

    return mean.toFixed(1);
}
