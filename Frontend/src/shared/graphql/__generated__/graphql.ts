export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  DateTime: { input: string; output: string; }
  UUID: { input: unknown; output: unknown; }
};

export type AirQualityReadingDto = {
  __typename: 'AirQualityReadingDto';
  co2: Scalars['Int']['output'];
  humidity: Scalars['Int']['output'];
  id: Scalars['UUID']['output'];
  locationId: Scalars['String']['output'];
  pm25: Scalars['Int']['output'];
  timestamp: Scalars['DateTime']['output'];
};

export type AirQualityReadingDtoFilterInput = {
  and?: InputMaybe<Array<AirQualityReadingDtoFilterInput>>;
  co2?: InputMaybe<IntOperationFilterInput>;
  humidity?: InputMaybe<IntOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  locationId?: InputMaybe<LocationTypeOperationFilterInput>;
  or?: InputMaybe<Array<AirQualityReadingDtoFilterInput>>;
  pm25?: InputMaybe<IntOperationFilterInput>;
  timestamp?: InputMaybe<DateTimeOperationFilterInput>;
};

export type AirQualityReadingDtoSortInput = {
  co2?: InputMaybe<SortEnumType>;
  humidity?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  locationId?: InputMaybe<SortEnumType>;
  pm25?: InputMaybe<SortEnumType>;
  timestamp?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type AirQualityReadingsConnection = {
  __typename: 'AirQualityReadingsConnection';
  /** A list of edges. */
  edges: Maybe<Array<AirQualityReadingsEdge>>;
  /** A flattened list of the nodes. */
  nodes: Maybe<Array<AirQualityReadingDto>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type AirQualityReadingsEdge = {
  __typename: 'AirQualityReadingsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: AirQualityReadingDto;
};

export type BooleanOperationFilterInput = {
  eq?: InputMaybe<Scalars['Boolean']['input']>;
  neq?: InputMaybe<Scalars['Boolean']['input']>;
};

export type DateTimeOperationFilterInput = {
  eq?: InputMaybe<Scalars['DateTime']['input']>;
  gt?: InputMaybe<Scalars['DateTime']['input']>;
  gte?: InputMaybe<Scalars['DateTime']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['DateTime']['input']>>>;
  lt?: InputMaybe<Scalars['DateTime']['input']>;
  lte?: InputMaybe<Scalars['DateTime']['input']>;
  neq?: InputMaybe<Scalars['DateTime']['input']>;
  ngt?: InputMaybe<Scalars['DateTime']['input']>;
  ngte?: InputMaybe<Scalars['DateTime']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['DateTime']['input']>>>;
  nlt?: InputMaybe<Scalars['DateTime']['input']>;
  nlte?: InputMaybe<Scalars['DateTime']['input']>;
};

export type EnergyAggregationDto = {
  __typename: 'EnergyAggregationDto';
  day: Scalars['DateTime']['output'];
  location: LocationType;
  totalEnergy: Scalars['Float']['output'];
};

/** A connection to a list of items. */
export type EnergyByDayConnection = {
  __typename: 'EnergyByDayConnection';
  /** A list of edges. */
  edges: Maybe<Array<EnergyByDayEdge>>;
  /** A flattened list of the nodes. */
  nodes: Maybe<Array<EnergyAggregationDto>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
};

/** An edge in a connection. */
export type EnergyByDayEdge = {
  __typename: 'EnergyByDayEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: EnergyAggregationDto;
};

export type EnergyReadingDto = {
  __typename: 'EnergyReadingDto';
  energy: Scalars['Float']['output'];
  id: Scalars['UUID']['output'];
  locationId: LocationType;
  timestamp: Scalars['DateTime']['output'];
};

export type EnergyReadingDtoFilterInput = {
  and?: InputMaybe<Array<EnergyReadingDtoFilterInput>>;
  energy?: InputMaybe<FloatOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  locationId?: InputMaybe<LocationTypeOperationFilterInput>;
  or?: InputMaybe<Array<EnergyReadingDtoFilterInput>>;
  timestamp?: InputMaybe<DateTimeOperationFilterInput>;
};

export type EnergyReadingDtoSortInput = {
  energy?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  locationId?: InputMaybe<SortEnumType>;
  timestamp?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type EnergyReadingsConnection = {
  __typename: 'EnergyReadingsConnection';
  /** A list of edges. */
  edges: Maybe<Array<EnergyReadingsEdge>>;
  /** A flattened list of the nodes. */
  nodes: Maybe<Array<EnergyReadingDto>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type EnergyReadingsEdge = {
  __typename: 'EnergyReadingsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: EnergyReadingDto;
};

export type FloatOperationFilterInput = {
  eq?: InputMaybe<Scalars['Float']['input']>;
  gt?: InputMaybe<Scalars['Float']['input']>;
  gte?: InputMaybe<Scalars['Float']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Float']['input']>>>;
  lt?: InputMaybe<Scalars['Float']['input']>;
  lte?: InputMaybe<Scalars['Float']['input']>;
  neq?: InputMaybe<Scalars['Float']['input']>;
  ngt?: InputMaybe<Scalars['Float']['input']>;
  ngte?: InputMaybe<Scalars['Float']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Float']['input']>>>;
  nlt?: InputMaybe<Scalars['Float']['input']>;
  nlte?: InputMaybe<Scalars['Float']['input']>;
};

export type IntOperationFilterInput = {
  eq?: InputMaybe<Scalars['Int']['input']>;
  gt?: InputMaybe<Scalars['Int']['input']>;
  gte?: InputMaybe<Scalars['Int']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Int']['input']>>>;
  lt?: InputMaybe<Scalars['Int']['input']>;
  lte?: InputMaybe<Scalars['Int']['input']>;
  neq?: InputMaybe<Scalars['Int']['input']>;
  ngt?: InputMaybe<Scalars['Int']['input']>;
  ngte?: InputMaybe<Scalars['Int']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Int']['input']>>>;
  nlt?: InputMaybe<Scalars['Int']['input']>;
  nlte?: InputMaybe<Scalars['Int']['input']>;
};

export enum LocationType {
  Bedroom = 'BEDROOM',
  Corridor = 'CORRIDOR',
  Garage = 'GARAGE',
  Kitchen = 'KITCHEN',
  LivingRoom = 'LIVING_ROOM',
  Office = 'OFFICE',
  Unknown = 'UNKNOWN'
}

export type LocationTypeOperationFilterInput = {
  eq?: InputMaybe<LocationType>;
  in?: InputMaybe<Array<LocationType>>;
  neq?: InputMaybe<LocationType>;
  nin?: InputMaybe<Array<LocationType>>;
};

export type MotionReadingDto = {
  __typename: 'MotionReadingDto';
  id: Scalars['UUID']['output'];
  locationId: LocationType;
  motionDetected: Scalars['Boolean']['output'];
  timestamp: Scalars['DateTime']['output'];
};

export type MotionReadingDtoFilterInput = {
  and?: InputMaybe<Array<MotionReadingDtoFilterInput>>;
  id?: InputMaybe<UuidOperationFilterInput>;
  locationId?: InputMaybe<LocationTypeOperationFilterInput>;
  motionDetected?: InputMaybe<BooleanOperationFilterInput>;
  or?: InputMaybe<Array<MotionReadingDtoFilterInput>>;
  timestamp?: InputMaybe<DateTimeOperationFilterInput>;
};

export type MotionReadingDtoSortInput = {
  id?: InputMaybe<SortEnumType>;
  locationId?: InputMaybe<SortEnumType>;
  motionDetected?: InputMaybe<SortEnumType>;
  timestamp?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type MotionReadingsConnection = {
  __typename: 'MotionReadingsConnection';
  /** A list of edges. */
  edges: Maybe<Array<MotionReadingsEdge>>;
  /** A flattened list of the nodes. */
  nodes: Maybe<Array<MotionReadingDto>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type MotionReadingsEdge = {
  __typename: 'MotionReadingsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: MotionReadingDto;
};

/** Information about pagination in a connection. */
export type PageInfo = {
  __typename: 'PageInfo';
  /** When paginating forwards, the cursor to continue. */
  endCursor: Maybe<Scalars['String']['output']>;
  /** Indicates whether more edges exist following the set defined by the clients arguments. */
  hasNextPage: Scalars['Boolean']['output'];
  /** Indicates whether more edges exist prior the set defined by the clients arguments. */
  hasPreviousPage: Scalars['Boolean']['output'];
  /** When paginating backwards, the cursor to continue. */
  startCursor: Maybe<Scalars['String']['output']>;
};

export type Query = {
  __typename: 'Query';
  airQualityReadings: Maybe<AirQualityReadingsConnection>;
  energyByDay: Maybe<EnergyByDayConnection>;
  energyReadings: Maybe<EnergyReadingsConnection>;
  motionReadings: Maybe<MotionReadingsConnection>;
};


export type QueryAirQualityReadingsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<AirQualityReadingDtoSortInput>>;
  where?: InputMaybe<AirQualityReadingDtoFilterInput>;
};


export type QueryEnergyByDayArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
};


export type QueryEnergyReadingsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<EnergyReadingDtoSortInput>>;
  where?: InputMaybe<EnergyReadingDtoFilterInput>;
};


export type QueryMotionReadingsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<MotionReadingDtoSortInput>>;
  where?: InputMaybe<MotionReadingDtoFilterInput>;
};

export enum SortEnumType {
  Asc = 'ASC',
  Desc = 'DESC'
}

export type UuidOperationFilterInput = {
  eq?: InputMaybe<Scalars['UUID']['input']>;
  gt?: InputMaybe<Scalars['UUID']['input']>;
  gte?: InputMaybe<Scalars['UUID']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['UUID']['input']>>>;
  lt?: InputMaybe<Scalars['UUID']['input']>;
  lte?: InputMaybe<Scalars['UUID']['input']>;
  neq?: InputMaybe<Scalars['UUID']['input']>;
  ngt?: InputMaybe<Scalars['UUID']['input']>;
  ngte?: InputMaybe<Scalars['UUID']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['UUID']['input']>>>;
  nlt?: InputMaybe<Scalars['UUID']['input']>;
  nlte?: InputMaybe<Scalars['UUID']['input']>;
};

export type GetAirQualityReadingsQueryVariables = Exact<{
  first?: InputMaybe<Scalars['Int']['input']>;
  where?: InputMaybe<AirQualityReadingDtoFilterInput>;
}>;


export type GetAirQualityReadingsQuery = { airQualityReadings: { __typename: 'AirQualityReadingsConnection', edges: Array<{ __typename: 'AirQualityReadingsEdge', node: { __typename: 'AirQualityReadingDto', id: unknown, timestamp: string, co2: number, pm25: number, humidity: number, locationId: string } }> | null } | null };

export type GetEnergyReadingsQueryVariables = Exact<{
  first?: InputMaybe<Scalars['Int']['input']>;
  where?: InputMaybe<EnergyReadingDtoFilterInput>;
}>;


export type GetEnergyReadingsQuery = { energyReadings: { __typename: 'EnergyReadingsConnection', edges: Array<{ __typename: 'EnergyReadingsEdge', node: { __typename: 'EnergyReadingDto', id: unknown, timestamp: string, energy: number, locationId: LocationType } }> | null } | null };

export type GetMotionReadingsQueryVariables = Exact<{
  first?: InputMaybe<Scalars['Int']['input']>;
  where?: InputMaybe<MotionReadingDtoFilterInput>;
}>;


export type GetMotionReadingsQuery = { motionReadings: { __typename: 'MotionReadingsConnection', edges: Array<{ __typename: 'MotionReadingsEdge', node: { __typename: 'MotionReadingDto', id: unknown, timestamp: string, motionDetected: boolean, locationId: LocationType } }> | null } | null };
