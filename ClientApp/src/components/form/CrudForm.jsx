import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import { 
  Alert, 
  Button, 
  Form, 
  Label,
  LabelFormItem,
  Loader,
} from '../../ui';
import { 
  ButtonList,
  FormHeader,
  StyledPage, 
  StyledWrapper,
} from '../../styles/layout/form';
import useQueryApiClient from '../../utils/useQueryApiClient';

export const CrudForm = ({
  form,
  url,
  formLabel,
  children
}) => {
  const { id } = useParams()
  const [successText, setSuccessText] = useState()
  const [errorText, setErrorText] = useState()

  const onSubmit = async (updateStatus, status = 'Confirmed') => {
    try {
      const values = await form.validateFields()

      if (!id) {
        createPost(values)
      }
    } catch (errorInfo) {
      form.scrollToField(errorInfo.errorFields[0]?.name, { behavior: 'smooth', block: 'center', scrollMode: 'if-needed' })
    }
  }

  const { appendData: createPost, isLoading: createLoading } = useQueryApiClient({
    request: {
      url: url,
      method: 'POST'
    },
    onSuccess: (response) => {
      setSuccessText(response.text ?? 'Ieraksts ir veiksmīgi izveidots.'); // TODO
    },
    onError: (error) => {
      setErrorText(error.text); // TODO
    }
  });

  return (
    <StyledPage>
      <Form form={form} >
        <FormHeader>
          <Label label={formLabel} extraBold />
          <LabelFormItem 
            label={'Status'} 
            labelValue={''}
          />
        </FormHeader>

        <Loader loading={createLoading} >
        
          { successText && 
            <Alert 
              message={successText}
              type="info"
              style={{ marginBottom: "20px" }}
            /> 
          }

          { 
            errorText && 
            <Alert 
              message={errorText}
              type="error"
              style={{ marginBottom: "20px" }}
            /> 
          }

          {children}

          <StyledWrapper>
            <ButtonList>
              <Button 
                htmlType="submit" 
                onClick={() => onSubmit(false)} 
                type="primary" 
                label={'Save'} 
              />
              <Button 
                htmlType="submit" 
                onClick={() => onSubmit(false)} 
                label={'Submit'} 
              />
            </ButtonList>
          </StyledWrapper>
        </Loader>
      </Form>
    </StyledPage>
  )
}
