import { type FC } from 'react';
import { Area, ComposedChart, Line, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts';
import { twMerge } from 'tailwind-merge';
import type { LineChartProps } from './types';

export const LineChart: FC<LineChartProps> = ({
  className,
  dataset,
  withGradient = false,
  isAnimationActive = true,
  labels,
}) => {

  const COLORS = [
    'var(--chart-1)',
    'var(--chart-2)',
    'var(--chart-3)'
  ];

  const dataKeys = dataset[0]
  ? Object.keys(dataset[0]).filter(k => k !== 'name')
  : [];

  const axisTick = { fill: 'var(--chart-tick)', fontSize: 12 };

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
              <stop offset='5%' stopColor='var(--chart-1)' stopOpacity={0.4} />
              <stop offset='95%' stopColor='var(--chart-1)' stopOpacity={0} />
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
            tick={axisTick}
            dataKey='name'
            axisLine={false}
            tickLine={false}
          />
          <YAxis
            tick={axisTick}
            domain={[0, 'dataMax']}
            minTickGap={0}
            type='number'
            tickMargin={10}
            axisLine={false}
            tickLine={false}
          />
          <Tooltip
            payloadUniqBy
            content={({ active, payload, label }) => {
              if (!active || !payload?.length) return null;

              return (
                <div className='rounded-lg border border-line bg-raised px-3 py-2 shadow-lg'>
                  <p className='text-sm text-secondary'>{label}</p>
                  {payload.map(entry => (
                    <p key={`tooltip-${entry.dataKey}`} className='text-sm text-regular'>
                      {labels?.[entry.dataKey] ?? entry.dataKey}: {entry.value}
                    </p>
                  ))}
                </div>
              );
            }}
          />
          {dataKeys.map((dataKey, index) => (
            <Line
              key={`${dataKey}-key`}
              isAnimationActive={isAnimationActive}
              name={labels?.[dataKey] ?? dataKey}
              dataKey={dataKeys[index]}
              stroke={COLORS[index]}
              dot={false}
            />))}
        </ComposedChart>
      </ResponsiveContainer>
    </div>
  );
};
