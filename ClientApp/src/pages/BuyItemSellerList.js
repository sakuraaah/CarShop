import React from 'react';
import {
  Label,
  LabelFormItem,
  List
} from '../ui';
import { 
  StyledPage
} from '../styles/layout/form';

export const BuyItemSellerList = () => {

  const renderTitle = (item) => {
    return (
      <>
        {`${item.mark} ${item.model} (${item.year})`}
      </>
    )
  }

  const renderDescription = (item) => {
    return (
      <>
        <Label 
          label={item.price} 
          listItem 
          extraBold
          currency
        />

        <br/>

        <Label 
          label={item.description} 
          listItem 
        />

        <br/>

        <LabelFormItem 
          label={'Mileage'} 
          labelValue={`${item.mileage} km`} 
        />
        <LabelFormItem 
          label={'Status'} 
          labelValue={item.status}
        />
      </>
    )
  }

  const filterItems = {
    addUser: false,
    addStatus: true,
    items: [
      [
        {
          label: 'Price from',
          name: 'PriceFrom',
          type: 'inputNumber'
        },
        {
          label: 'Price to',
          name: 'PriceTo',
          type: 'inputNumber'
        }
      ],
      [
        {
          label: 'Mileage from',
          name: 'MileageFrom',
          type: 'inputNumber'
        },
        {
          label: 'Mileage to',
          name: 'MileageTo',
          type: 'inputNumber'
        }
      ],
      [
        {
          label: 'Year from',
          name: 'YearFrom',
          type: 'dateYear'
        },
        {
          label: 'Year to',
          name: 'YearTo',
          type: 'dateYear'
        }
      ],
      [
        {
          label: 'Engine power from',
          name: 'EngPowerFrom',
          type: 'inputNumber'
        },
        {
          label: 'Engine power to',
          name: 'EngPowerTo',
          type: 'inputNumber'
        }
      ],
      [
        {
          label: 'Category',
          name: 'Category',
          type: 'select',
          apiUrl: 'api/categories'
        },
        {
          label: 'Mark',
          name: 'Mark',
          type: 'select',
          apiUrl: 'api/marks'
        },
        {
          label: 'Model',
          name: 'Model',
          type: 'input'
        },
        {
          label: 'Body type',
          name: 'BodyType',
          type: 'select',
          apiUrl: 'api/body-types'
        }
      ],
      [
        {
          label: 'Color',
          name: 'Color',
          type: 'select',
          apiUrl: 'api/colors'
        },
        {
          label: 'Seats',
          name: 'Seats',
          type: 'inputNumber'
        }
      ],
      [
        {
          label: 'Features',
          name: 'FeatureList',
          type: 'checkboxes',
          apiUrl: 'api/features'
        }
      ]
    ]
  }

  const sortItems = [
    {
      label: 'Status',
      value: 'Status'
    },
    {
      label: 'Price',
      value: 'Price'
    },
    {
      label: 'Mileage',
      value: 'Mileage'
    },
    {
      label: 'Year',
      value: 'Year'
    },
    {
      label: 'Category',
      value: 'Category'
    },
    {
      label: 'Mark',
      value: 'Mark'
    },
    {
      label: 'Model',
      value: 'Model'
    },
    {
      label: 'BodyType',
      value: 'BodyType'
    }
  ]

  const createNewButton = {
    label: 'Create new Buy item',
    url: '/new-buy-item'
  }

  return (
    <StyledPage>
      <List
        title={'Your listed vehicles for sale'}
        url={'buy-item'}
        apiUrl={'api/buy-items/seller'}
        button={createNewButton}
        filterItems={filterItems}
        sortItems={sortItems}
        renderTitle={renderTitle}
        renderDescription={renderDescription}
      />
    </StyledPage>
  )
}
