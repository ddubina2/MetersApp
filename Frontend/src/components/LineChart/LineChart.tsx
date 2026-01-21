import { type FC } from 'react';
import { Area, CartesianGrid, ComposedChart, Line, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts';
import { twMerge } from 'tailwind-merge';
import type { LineChartProps } from './types';

export const LineChart: FC<LineChartProps> = ({
  className,
  dataset,
  withGradient = false,
  isAnimationActive = true,
}) => {

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
          <CartesianGrid vertical={false} stroke='#F3F4F6' strokeDasharray='0' />
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
          {dataKeys[0] ? <Line isAnimationActive={isAnimationActive} dataKey={dataKeys[0]} stroke='#7996E0' dot={false} /> : null}

          {dataKeys[1] ? <Line isAnimationActive={isAnimationActive} dataKey={dataKeys[1]} stroke='#82ca9d' dot={false} /> : null}

          {dataKeys[2] ? <Line isAnimationActive={isAnimationActive} dataKey={dataKeys[2]} stroke='#E0C879' dot={false} /> : null}
        </ComposedChart>
      </ResponsiveContainer>
    </div>
  );
};
