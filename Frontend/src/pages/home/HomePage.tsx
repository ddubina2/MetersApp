import { LocationType } from '@shared/graphql/__generated__/graphql';
import { Tabs } from '@components/Tabs';
import { LocationTab } from '@widgets/LocationTab';

export const HomePage = () => {

  return (
    <div className='min-h-screen bg-surface p-5'>
      <Tabs
        className='w-full'
        headerClassName='justify-center'
        buttonClassName='aria-selected:border-primary'
        items={Object.entries(LocationType)
          .filter(([, value]) => value !== LocationType.Unknown)
          .map(([key, value]) => ({
            title: key === 'LivingRoom' ? 'Living Room' : key,
            element: <LocationTab type={value} />,
          }))
        }
      />
    </div>
  );
};

