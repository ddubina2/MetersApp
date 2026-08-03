import { type FC } from 'react';
import { Area, ComposedChart, Line, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts';
import { twMerge } from 'tailwind-merge';
import type { LineChartProps } from './types';

export const LineChart: FC<LineChartProps> = ({
  className,
  dataset,
  withGradient = false,
  isAnimationActive = true,
}) => {

  const COLORS = [
    '#7996E0',
    '#82ca9d',
    '#E0C879'
  ];

  const dataKeys = dataset[0]
  ? Object.keys(dataset[0]).filter(k => k !== 'name')
  : [];

  return (
    <div className={twMerge('h-[234px] w-[1000px] min-w-[200px]', className)} data-testid='line-chart-container'>
      <ResponsiveContainer width='100%' height='100%' minHeight={234} minWidth={200}>
        <ComposedChart
          data={dataset}
          margin={{
            top: 5,
            right: 30,
            left: 20,
            bottom: 5,
          }}
        >
          <defs>
            <linearGradient id='colorValue' x1='0' y1='0' x2='0' y2='1'>
              <stop offset='5%' stopColor='#7996E0' stopOpacity={0.4} />
              <stop offset='95%' stopColor='#7996E0' stopOpacity={0} />
            </linearGradient>
          </defs>
          <Area
            type='linear'
            isAnimationActive={isAnimationActive}
            dataKey={dataKeys[0]}
            stroke='transparent'
            fill={withGradient ? 'url(#colorValue)' : 'transparent'}
          />
          <XAxis
            tick={{ fill: '#18181B', fontSize: 12 }}
            dataKey='name'
            axisLine={false}
            tickLine={false}
          />
          <YAxis
            tick={{ fill: '#18181B', fontSize: 12 }}
            domain={[0, 'dataMax']}
            minTickGap={0}
            type='number'
            tickMargin={10}
            axisLine={false}
            tickLine={false}
          />
          <Tooltip />
          {dataKeys.map((dataKey, index) => (
            <Line
              key={`${dataKey}-key`}
              isAnimationActive={isAnimationActive}
              dataKey={dataKeys[index]}
              stroke={COLORS[index]}
              dot={false}
            />))}
        </ComposedChart>
      </ResponsiveContainer>
    </div>
  );
};
