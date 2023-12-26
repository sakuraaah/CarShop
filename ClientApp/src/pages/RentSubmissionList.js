import React from 'react';
import {
  Label,
  LabelFormItem,
  List
} from '../ui';
import { 
  StyledPage
} from '../styles/layout/form';

export const RentSubmissionList = () => {

  const renderTitle = (item) => {
    return (
      <>
        {`${item.mark} ${item.model}`}
      </>
    )
  }

  const renderDescription = (item) => {
    return (
      <>
        <Label 
          label={item.category} 
          listItem 
          extraBold 
        />
        <LabelFormItem 
          label={'Seller'} 
          labelValue={item.user.userName}
        />
        <br />
        <LabelFormItem 
          label={'Technical passport number'} 
          labelValue={item.aplNr}
        />
        <LabelFormItem 
          label={'License plate number'} 
          labelValue={item.regNr}
        />
        <LabelFormItem 
          label={'Status'} 
          labelValue={item.status}
        />
        <LabelFormItem 
          label={'Admin status'} 
          labelValue={item.adminStatus}
        />
      </>
    )
  }

  const filterItems = {
    addUser: true,
    addStatus: true,
    items: [
      [
        {
          label: 'Technical passport number',
          name: 'AplNr',
          type: 'input'
        },
        {
          label: 'License plate number',
          name: 'RegNr',
          type: 'input'
        }
      ],
      [
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
          label: 'Category',
          name: 'Category',
          type: 'select',
          apiUrl: 'api/categories'
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
      label: 'Technical passport number',
      value: 'AplNr'
    },
    {
      label: 'License plate number',
      value: 'RegNr'
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
      label: 'Mileage',
      value: 'Mileage'
    },
    {
      label: 'Year',
      value: 'Year'
    }
  ]

  return (
    <StyledPage>
      <List
        title={'Rent submission List'}
        url={'rent-submission'}
        apiUrl={'api/rent-submissions'}
        filterItems={filterItems}
        sortItems={sortItems}
        renderTitle={renderTitle}
        renderDescription={renderDescription}
      />
    </StyledPage>
  )
}
