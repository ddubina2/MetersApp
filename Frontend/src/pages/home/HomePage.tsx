import { LocationType } from '@shared/graphql/__generated__/graphql';
import { Tabs } from '@components/Tabs';
import { LocationTab } from '@widgets/LocationTab';
import { useTranslation } from 'react-i18next';
import type { ParseKeys } from 'i18next';

const locationTabKey: Record<LocationType, ParseKeys> = {
  [LocationType.Bedroom]: 'home.tabs.bedroom',
  [LocationType.Corridor]: 'home.tabs.corridor',
  [LocationType.Garage]: 'home.tabs.garage',
  [LocationType.Kitchen]: 'home.tabs.kitchen',
  [LocationType.LivingRoom]: 'home.tabs.livingRoom',
  [LocationType.Office]: 'home.tabs.office',
  [LocationType.Unknown]: 'home.tabs.livingRoom',
};

export const HomePage = () => {
  const { t } = useTranslation();

  return (
    <Tabs
      className='w-full'
      headerClassName='justify-center'
      buttonClassName='aria-selected:border-primary'
      textClassName='font-semibold'
      items={Object.entries(LocationType)
        .filter(([, value]) => value !== LocationType.Unknown)
        .map(([, value]) => ({
          title: t(locationTabKey[value]),
          element: <LocationTab type={value} />,
        }))
      }
    />
  );
};

