import React from 'react';
import {
  Label,
  LabelFormItem,
  List
} from '../ui';
import { 
  StyledPage
} from '../styles/layout/form';

export const RentItemList = () => {

  const renderTitle = (item) => {
    return (
      <>
        {`${item.mark !== 'Other' ? item.mark : ''} ${item.model}`}
      </>
    )
  }

  const renderDescription = (item) => {
    return (
      <>
        <Label 
          label={item.price}
          postLabel={item.rentCategory === 'Daily' ? 'day' : 'min'}
          listItem 
          extraBold
          currency
        />

        <br/>

        <Label 
          label={item.carClass} 
          listItem 
          extraBold
        />

        <br/>

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
          label: 'Rental Category',
          name: 'RentCategory',
          type: 'select',
          apiUrl: 'api/rent-categories'
        }
      ],
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
          label: 'Class',
          name: 'CarClass',
          type: 'select',
          apiUrl: 'api/car-classes'
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
      label: 'Class',
      value: 'CarClass'
    }
  ]

  return (
    <StyledPage>
      <List
        title={'Rent a vehicle'}
        url={'rent-item'}
        apiUrl={'api/rent-items'}
        filterItems={filterItems}
        sortItems={sortItems}
        renderTitle={renderTitle}
        renderDescription={renderDescription}
      />
    </StyledPage>
  )
}
