import React from 'react';
import {
  Label,
  LabelFormItem,
  List
} from '../ui';
import { 
  StyledPage
} from '../styles/layout/form';

export const RentItemSellerList = () => {

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
          labelValue={item.user.status}
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
      label: 'Status',
      value: 'Status'
    },
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
        title={'Your listed rentals'}
        url={'rent-item'}
        apiUrl={'api/rent-items/seller'}
        filterItems={filterItems}
        sortItems={sortItems}
        renderTitle={renderTitle}
        renderDescription={renderDescription}
      />
    </StyledPage>
  )
}
