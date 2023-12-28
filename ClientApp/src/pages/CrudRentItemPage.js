import React, { useState, useEffect } from 'react';
import { useParams, useLocation, useNavigate } from 'react-router-dom';
import { Form as AntdForm } from 'antd';
import { CrudForm } from '../components/form/CrudForm';
import {
  CheckboxGroup,
  DatePicker,
  ImageUpload,
  Input,
  InputNumber,
  Label,
  Loader,
  Select,
  SideBySide, 
} from '../ui';
import { 
  BorderBottom,
  StyledPage, 
  StyledWrapper,
} from '../styles/layout/form';
import dayjs from 'dayjs';
import useQueryApiClient from '../utils/useQueryApiClient';

export const CrudRentItemPage = () => {
  const [form] = AntdForm.useForm();

  const [disabled, setDisabled] = useState(false);
  const [imageUrl, setImageUrl] = useState();

  const { id } = useParams()
  const navigate = useNavigate()
  const location = useLocation()

  useEffect(() => {
    if (!id && location.state?.rentSubmissionId) {
      getRentSubmissionFields()
    }
  }, [id, location])

  const { refetch: getRentSubmissionFields, isLoading: getRentSubmissionLoading } = useQueryApiClient({
    request: {
      url: `api/rent-submissions/${location.state?.rentSubmissionId}`,
      method: 'GET',
      disableOnMount: true,
    },
    onSuccess: (response) => {
      let values = response.data

      setImageUrl(values.imgSrc)

      form.setFieldsValue({
        imgSrc: values.imgSrc,
        aplNr: values.aplNr,
        regNr: values.regNr,
        categoryId: values.categoryId,
        markId: values.markId,
        model: values.model,
        mileage: values.mileage,
        year: dayjs().set('year', values.year)
      })
    },
    onError: () => {
      navigate(-1)
    }
  });

  const parseFormToSubmit = (values) => {
    return {
      rentSubmissionId: location.state?.rentSubmissionId,

      bodyTypeId: values.bodyTypeId,
      price: values.price,
      rentCategoryId: values.rentCategoryId,
      carClassId: values.carClassId,
      colorId: values.colorId,
      seats: values.seats,

      features: values.features.map((feature) => ({
        name: feature
      })),
    }
  }

  const parseResponseToForm = (values) => {
    setImageUrl(values.imgSrc)

    return {
      imgSrc: values.imgSrc,
      aplNr: values.aplNr,
      regNr: values.regNr,
      categoryId: values.categoryId,
      markId: values.markId,
      model: values.model,
      mileage: values.mileage,
      year: dayjs().set('year', values.year),

      bodyTypeId: values.bodyTypeId,
      price: values.price,
      rentCategoryId: values.rentCategoryId,
      carClassId: values.carClassId,
      colorId: values.colorId,
      seats: values.seats,

      features: values.features.map((feature) => feature.name)
    }
  }

  return (
    <StyledPage>
      <CrudForm 
        form={form}
        url={'rent-item'}
        apiUrl={'api/rent-items'}
        name={'Rent item'}
        type={'Rental'}
        parseFormToSubmit={parseFormToSubmit}
        parseResponseToForm={parseResponseToForm}
        disabled={disabled}
        setDisabled={setDisabled}
      >
        <StyledWrapper>
          <Loader loading={getRentSubmissionLoading}>

            <Label label={'Info from Rent submisison:'} extraBold />

            <BorderBottom />

            <ImageUpload
              form={form}
              imageUrl={imageUrl}
              setImageUrl={setImageUrl}
              name="imgSrc"
              rules={[{ required: true }]}
              disabled={true}
            />

            <BorderBottom />

            <SideBySide
              left={
                <Input
                  name="aplNr"
                  label={'Technical passport number'}
                  placeholder="AB1234567"
                  rules={[{ required: true }]}
                  disabled
                />
              }
              right={
                <Input
                  name="regNr"
                  label={'License plate number'}
                  placeholder="LV1234"
                  rules={[{ required: true }]}
                  disabled
                />
              }
            />

            <SideBySide
              left={
                <Select
                  name="categoryId"
                  label={'Category'}
                  url={'api/categories'}
                  rules={[{ required: true }]}
                  disabled
                />
              }
            />

            <SideBySide
              left={
                <Select
                  name="markId"
                  label={'Mark'}
                  url={'api/marks'}
                  rules={[{ required: true }]}
                  disabled
                />
              }
              right={
                <Input
                  name="model"
                  label={'Model'}
                  rules={[{ required: true }]}
                  disabled
                />
              }
            />

            <SideBySide
              left={
                <InputNumber
                  name="mileage"
                  label={'Mileage'}
                  min={0}
                  precision={0}
                  addonAfter={"km"}
                  rules={[{ required: true }]}
                  disabled
                />
              }
            />

            <SideBySide
              left={
                <DatePicker
                  name="year"
                  label={'Year'}
                  picker="year"
                  disabledDate={(date) => date > dayjs()}
                  rules={[{ required: true }]}
                  disabled
                />
              }
            />

          </Loader>
        </StyledWrapper>

        <StyledWrapper>
          <Label label={'Product details:'} extraBold />

          <BorderBottom />

          <SideBySide
            left={
              <InputNumber
                name="price"
                label={'Price'}
                min={0}
                precision={2}
                addonAfter={"€"}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <Select
                name="rentCategoryId"
                label={'Rental category'}
                url={'api/rent-categories'}
                rules={[{ required: true }]}
              />
            }
            right={
              <Select
                name="carClassId"
                label={'Class'}
                url={'api/car-classes'}
                rules={[{ required: true }]}
              />
            }
          />
        </StyledWrapper>

        <StyledWrapper>
          <Label label={'Additional info:'} extraBold />

          <BorderBottom />

          <SideBySide
            left={
              <Select
                name="bodyTypeId"
                label={'Body type'}
                url={'api/body-types'}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <Select
                name="colorId"
                label={'Color'}
                url={'api/colors'}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <InputNumber
                name="seats"
                label={'Seats'}
                min={1}
                max={9}
                precision={0}
                rules={[{ required: true }]}
              />
            }
          />

          <BorderBottom />

          <SideBySide
            left={
              <CheckboxGroup
                name="features"
                label={'Features'}
                url={'api/features'}
                sameAsLabel
              />
            }
          />
        </StyledWrapper>
      </CrudForm>
    </StyledPage>
  )
}
