import { ChartDataset } from "chart.js";

export const calculerMoyenne = (data: ChartDataset) => {
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
