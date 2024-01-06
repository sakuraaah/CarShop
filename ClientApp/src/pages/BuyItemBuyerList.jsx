import React from 'react';
import {
  Label,
  LabelFormItem,
  List
} from '../ui';
import { 
  StyledPage
} from '../styles/layout/form';

export const BuyItemBuyerList = () => {

  const renderTitle = (item) => {
    return (
      <>
        {`${item.mark !== 'Other' ? item.mark : ''} ${item.model} (${item.year})`}
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

  return (
    <StyledPage>
      <List
        title={'Your bought vehicles'}
        url={'buy-item'}
        apiUrl={'api/buy-items/buyer'}
        renderTitle={renderTitle}
        renderDescription={renderDescription}
      />
    </StyledPage>
  )
}
