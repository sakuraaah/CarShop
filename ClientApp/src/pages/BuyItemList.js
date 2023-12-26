import React from 'react';
import {
  Label,
  LabelFormItem,
  List
} from '../ui';
import { 
  StyledPage
} from '../styles/layout/form';

export const BuyItemList = () => {

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
          label={'Seller'} 
          labelValue={item.user.userName}
        />
      </>
    )
  }

  const filterItems = {
    addUser: true,
    addStatus: false,
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

  return (
    <StyledPage>
      <List
        title={'Buy a vehicle'}
        url={'buy-item'}
        apiUrl={'api/buy-items'}
        filterItems={filterItems}
        sortItems={sortItems}
        renderTitle={renderTitle}
        renderDescription={renderDescription}
      />
    </StyledPage>
  )
}
