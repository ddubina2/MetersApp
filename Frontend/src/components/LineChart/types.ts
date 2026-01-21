export type LineChartData = {
  name: string;
  [key: string]: unknown;
};

export type LineChartProps = {
  className?: string;
  withGradient?: boolean;
  dataset: LineChartData[];
  isAnimationActive?: boolean;
}
